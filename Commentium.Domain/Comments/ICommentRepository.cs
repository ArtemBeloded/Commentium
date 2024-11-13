namespace Commentium.Domain.Comments
{
    public interface ICommentRepository
    {
        Task Add(Comment comment);

        IQueryable<Comment> GetComments();
    }
}
