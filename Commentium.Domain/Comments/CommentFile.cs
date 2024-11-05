using Commentium.Domain.Shared;
using static Commentium.Domain.Errors.DomainErrors;

namespace Commentium.Domain.Comments
{
    public class CommentFile
    {
        private const int MaxTextFileLenght = 100 * 1024;

        private CommentFile(
            Guid id,
            Guid commentId,
            string fileName,
            string contentType,
            byte[] content) 
        {
            Id = id;
            CommentId = commentId;
            FileName = fileName;
            ContentType = contentType;
            Content = content;
        }

        private CommentFile() { }

        public Guid Id { get; private set; }

        public Guid CommentId { get; private set; }

        public string FileName { get; private set; }

        public string ContentType { get; private set; }

        public byte[] Content { get; private set; }

        public static Result<CommentFile> Create(
            Guid commentId,
            string fileName,
            string contentType,
            byte[] content)
        {
            var isCommentFileCorrectResult = IsCommentFileCorrect(fileName, contentType, content);

            if (isCommentFileCorrectResult.IsFailure) 
            {
                return Result.Failure<CommentFile>(isCommentFileCorrectResult.Error);
            }

            var commentFile = new CommentFile(
                Guid.NewGuid(),
                commentId,
                fileName,
                contentType,
                content);

            return commentFile;
        }

        private static Result<bool> IsCommentFileCorrect(
            string fileName,
            string contentType,
            byte[] content)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();
            bool isImageFile = fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".gif" || fileExtension == ".png";
            bool isTextFile = fileExtension == ".txt";

            if (isImageFile)
            {
                //to do: verificaton??
            }
            else if (isTextFile)
            {
                var isCommentTextFileCorrectResult = IsCommentTextFileCorrect(content);

                if (isCommentTextFileCorrectResult.IsFailure) 
                {
                    return Result.Failure<bool>(isCommentTextFileCorrectResult.Error);
                }
            }
            else 
            {
                return Result.Failure<bool>(CommentFileErrors.InvalidFileFormat);
            }

            return true;
        }

        private static Result<bool> IsCommentTextFileCorrect(byte[] content) 
        {
            if (content.Length > MaxTextFileLenght) 
            {
                return Result.Failure<bool>(CommentFileErrors.InvalidTextFileSize);
            }

            return true;
        }
    }
}
