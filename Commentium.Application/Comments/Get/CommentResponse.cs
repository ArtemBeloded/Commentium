using Commentium.Domain.Comments;

namespace Commentium.Application.Comments.Get
{
    public record CommentResponse(
        Guid CommentId,
        string UserName,
        string UserEmail,
        string Text,
        CommentFile? AttachedFile,
        DateTime CreatedDate,
        List<CommentResponse> Replies);
}
