using Microsoft.AspNetCore.Mvc;

namespace Commentium.API.Endpoints
{
    [Route("api/comments")]
    public sealed class CommentsController : Controller
    {
        [HttpPost("addcomment")]
        public async Task<IResult> AddComment() 
        {
            return Results.Ok();
        }

        [HttpGet]
        public async Task<IResult> GetComments() 
        {
            return Results.Ok();
        }

        [HttpGet("{id:guid}")]
        public async Task<IResult> GetComment(Guid commentId) 
        {
            return Results.Ok();
        }
    }
}
