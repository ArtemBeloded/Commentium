using Commentium.Domain.Shared;
using Commentium.Domain.Users;
using Ganss.Xss;
using static Commentium.Domain.Errors.DomainErrors;

namespace Commentium.Domain.Comments
{
    public class Comment
    {
        private Comment(
            Guid id,
            Guid userId,
            DateTime createdDate,
            string text,
            Guid? parentCommentId)
        {
            Id = id;
            UserId = userId;
            CreatedDate = createdDate;
            Text = text;
            ParentCommentId = parentCommentId;
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
            string text,
            Guid? parentCommentId)
        {
            var isTextCorrectResult = IsTextCorrect(text);

            if (isTextCorrectResult.IsFailure) 
            {
                return Result.Failure<Comment>(isTextCorrectResult.Error);
            }

            var comment = new Comment(
                Guid.NewGuid(),
                userId,
                DateTime.Now,
                text,
                parentCommentId);

            return comment;
        }

        public void AttachFile(CommentFile file) 
        {
            AttachedFile = file;
        }

        private static Result<bool> IsTextCorrect(string text) 
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return Result.Failure<bool>(CommentContentErrors.Empty);
            }

            var sanitizer = new HtmlSanitizer();
            sanitizer.AllowedTags.Add("a");
            sanitizer.AllowedTags.Add("code");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("strong");
            sanitizer.AllowedAttributes.Add("href");
            sanitizer.AllowedAttributes.Add("title");

            var sanitizedText = sanitizer.Sanitize(text);

            if (sanitizedText != text)
            {
                return Result.Failure<bool>(CommentContentErrors.InvalidHTMLTags);
            }

            return true;
        } 
    }
}
