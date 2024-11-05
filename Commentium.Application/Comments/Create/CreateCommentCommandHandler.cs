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

        public CreateCommentCommandHandler(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
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

            var commentCreateResult = Comment.Create(user.Id, request.Text);

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

            return Result.Success();
        }
    }
}
