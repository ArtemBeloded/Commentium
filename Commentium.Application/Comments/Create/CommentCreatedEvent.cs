namespace Commentium.Application.Comments.Create
{
    public record CommentCreatedEvent
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string CommentContent { get; set; } = string.Empty;

        public DateTime CreatedDate { get; set; }

        public bool IsFileAttached { get; set; }
    }
}
