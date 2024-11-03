using Commentium.Domain.Shared;
using MediatR;

namespace Commentium.Application.Comments.Create
{
    public record CreateCommentCommand(
        string UserName,
        string Email,
        string Text) : IRequest<Result>;
}
