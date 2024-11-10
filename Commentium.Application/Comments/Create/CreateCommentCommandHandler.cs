using Commentium.Application.Abstractions.Caching;
using Commentium.Application.Abstractions.EventBus;
using Commentium.Domain.Comments;
using Commentium.Domain.Shared;
using Commentium.Domain.Users;
using MediatR;
using static Commentium.Domain.Errors.DomainErrors;

namespace Commentium.Application.Comments.Create
{
    public sealed class CreateCommentCommandHandler
        : IRequestHandler<CreateCommentCommand, Result>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly IEventBus _eventBus;

        public CreateCommentCommandHandler(
            ICommentRepository commentRepository,
            IUserRepository userRepository,
            ICacheService cacheService,
            IEventBus eventBus)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
            _cacheService = cacheService;
            _eventBus = eventBus;
        }

        public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmail(request.Email);

            if (user is null)
            {
                var userCreateResult = User.Create(request.UserName, request.Email);

                if (userCreateResult.IsFailure)
                {
                    return Result.Failure(userCreateResult.Error);
                }

                await _userRepository.Add(userCreateResult.Value);

                user = userCreateResult.Value;
            }
            else if (user.UserName != request.UserName) 
            {
                return Result.Failure(UserErrors.InvalidUserName);
            }

            var commentCreateResult = Comment.Create(user.Id, request.Text, request.ParentCommentId);

            if (commentCreateResult.IsFailure) 
            {
                return Result.Failure(commentCreateResult.Error);
            }

            var comment = commentCreateResult.Value;

            if (request.File is not null) 
            {
                var commentFileResult = CommentFile.Create(
                    comment.Id,
                    request.File.FileName,
                    request.File.ContentType,
                    request.File.FileData);

                if (commentFileResult.IsFailure) 
                {
                    return Result.Failure(commentFileResult.Error);
                }

                comment.AttachFile(commentFileResult.Value);
            }

            await _commentRepository.Add(comment);

            await _cacheService.RemoveByPrefixAsync("comments", cancellationToken);

            await _eventBus.PublishAsync(
                new CommentCreatedEvent
                {
                    Id = comment.Id,
                    UserName = user.UserName,
                    CommentContent = comment.Text,
                    CreatedDate = comment.CreatedDate,
                    IsFileAttached = comment.AttachedFile is not null
                }, cancellationToken);

            return Result.Success();
        }
    }
}
