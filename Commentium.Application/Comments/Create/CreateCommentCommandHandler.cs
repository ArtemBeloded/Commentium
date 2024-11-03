using Commentium.Domain.Comments;
using Commentium.Domain.Shared;
using Commentium.Domain.Users;
using MediatR;

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
            //check if user with this user name and email exist???
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

            var commentCreateResult = Comment.Create(user.Id, request.Text);

            if (commentCreateResult.IsFailure) 
            {
                return Result.Failure(commentCreateResult.Error);
            }

            await _commentRepository.Add(commentCreateResult.Value);

            return Result.Success();
        }
    }
}
