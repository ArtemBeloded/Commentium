namespace Commentium.API.Contracts
{
    public record AddCommentRequest(
        string UserName,
        string Email,
        string Text,
        IFormFile? File);
}
