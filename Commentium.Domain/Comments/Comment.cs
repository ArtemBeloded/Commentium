using Commentium.Domain.Shared;
using Commentium.Domain.Users;
using System.Text.RegularExpressions;
using System.Xml;
using static Commentium.Domain.Errors.DomainErrors;

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
            var isTextCorrectResult = IsTextCorrect(text);

            if (isTextCorrectResult.IsFailure) 
            {
                return Result.Failure<Comment>(isTextCorrectResult.Error);
            }

            var comment = new Comment(
                Guid.NewGuid(),
                userId,
                DateTime.Now,
                text);

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

            var allowedTagsPattern = @"^([^<>]*|(<a\s+href=""[^""]*""(\s+title=""[^""]*"")?>[^<>]*<\/a>)|(<code>[^<>]*<\/code>)|(<i>[^<>]*<\/i>)|(<strong>[^<>]*<\/strong>))*$";
            //to do: change text pattern
            //test cases
            //4 < 5 - true,
            //<strong>Жирный текст</strong> и 4 < 5 - true,
            //<a href=\"https://example.com\">ссылка</a> и 3 > 2 - true,
            //<b>неразрешенный тег</b> - false,
            //<code>int x = 5; < 10</code> - true

            var isValid = Regex.IsMatch(text, allowedTagsPattern, RegexOptions.Singleline);

            if (!isValid)
            {
                return Result.Failure<bool>(CommentContentErrors.InvalidHTMLTags);
            }

            try
            {
                var wrappedText = $"<root>{text}</root>";
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(wrappedText);

                return true;
            }
            catch (XmlException)
            {
                return Result.Failure<bool>(CommentContentErrors.UnclosedHTMLTags);
            }
        } 
    }
}
