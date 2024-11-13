using Commentium.Domain.Shared;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
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
                isCommentFileCorrectResult.Value);

            return commentFile;
        }

        private static Result<byte[]> IsCommentFileCorrect(
            string fileName,
            string contentType,
            byte[] content)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower();
            bool isImageFile = fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".gif" || fileExtension == ".png";
            bool isTextFile = fileExtension == ".txt";

            if (isImageFile)
            {
                var processedImageResult = ResizingFile(content, fileExtension);
                if (processedImageResult.IsFailure)
                {
                    return Result.Failure<byte[]>(processedImageResult.Error);
                }
                return processedImageResult.Value;
            }
            else if (isTextFile)
            {
                var isCommentTextFileCorrectResult = IsCommentTextFileCorrect(content);

                if (isCommentTextFileCorrectResult.IsFailure) 
                {
                    return Result.Failure<byte[]>(isCommentTextFileCorrectResult.Error);
                }

                return content;
            }
            else 
            {
                return Result.Failure<byte[]>(CommentFileErrors.InvalidFileFormat);
            }
        }

        private static Result<byte[]> ResizingFile (byte[] content, string fileExtension)
        {
            using (var memoryStream = new MemoryStream(content))
            {
                using (var image = Image.Load(memoryStream))
                {
                    int maxWidth = 320;
                    int maxHeight = 240;

                    if (image.Width > maxWidth || image.Height > maxHeight)
                    {
                        var ratioX = (double)maxWidth / image.Width;
                        var ratioY = (double)maxHeight / image.Height;
                        var ratio = Math.Min(ratioX, ratioY);

                        int newWidth = (int)(image.Width * ratio);
                        int newHeight = (int)(image.Height * ratio);

                        image.Mutate(x => x.Resize(newWidth, newHeight));
                    }

                    using (var resultStream = new MemoryStream())
                    {
                        switch (fileExtension)
                        {
                            case ".jpg":
                            case ".jpeg":
                                image.Save(resultStream, new JpegEncoder());
                                break;
                            case ".png":
                                image.Save(resultStream, new PngEncoder());
                                break;
                            case ".gif":
                                image.Save(resultStream, new GifEncoder());
                                break;
                            default:
                                image.Save(resultStream, new JpegEncoder());
                                break;
                        }
                        return resultStream.ToArray();
                    }
                }
            }
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
