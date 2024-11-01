namespace Commentium.Domain.Comments
{
    public class Comment
    {
        public Guid Id { get; private set; }

        public string Text { get; private set; }

        public CommentFile? AttachedFile { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public Guid UserId { get; private set; }

        public Guid? ParentCommentId { get; private set; }

        public ICollection<Comment> Replies { get; private set; } = new List<Comment>();

    }
}
