using Commentium.Domain.Shared;
using Commentium.Domain.Users;

namespace Commentium.Domain.Comments
{
    public class Comment
    {
        private Comment(
            Guid id,
            Guid userId,
            DateTime createdDate,
            string text)
        {
            Id = id;
            UserId = userId;
            CreatedDate = createdDate;
            Text = text;
        }

        public Guid Id { get; private set; }

        public string Text { get; private set; }

        public CommentFile? AttachedFile { get; private set; }

        public DateTime CreatedDate { get; private set; }

        public Guid UserId { get; private set; }

        public User User { get; private set; }

        public Guid? ParentCommentId { get; private set; }

        public ICollection<Comment> Replies { get; private set; } = new List<Comment>();

        public static Result<Comment> Create(
            Guid userId,
            string text)
        {
            //Credential verification

            var comment = new Comment(
                Guid.NewGuid(),
                userId,
                DateTime.Now,
                text);

            return comment;
        }
    }
}
