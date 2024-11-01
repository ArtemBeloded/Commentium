namespace Commentium.Domain.Comments
{
    public class CommentFile
    {
        public Guid Id { get; private set; }

        public Guid CommentId { get; private set; }

        public string FileName { get; private set; }

        public string ContentType { get; private set; }

        public byte[] Content { get; private set; }
    }
}
