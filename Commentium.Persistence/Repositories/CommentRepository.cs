using Commentium.Domain.Comments;

namespace Commentium.Persistence.Repositories
{
    internal sealed class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Add(Comment comment)
        {
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
        }

        public IQueryable<Comment> GetComments()
        {
            IQueryable<Comment> comments = _context.Comments;

            return comments;
        }
    }
}
