using Commentium.Domain.Shared;
using MediatR;

namespace Commentium.Application.Comments.Create
{
    public record CreateCommentCommand(
        string UserName,
        string Email,
        string Text,
        Guid? ParentCommentId,
        CreateCommentFileCommand? File) : IRequest<Result>;

    public record CreateCommentFileCommand(
        string FileName,
        string ContentType,
        byte[] FileData);
}
