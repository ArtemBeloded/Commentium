using Commentium.API.Contracts;
using Commentium.API.Extensions;
using Commentium.Application.Comments.Create;
using Commentium.Application.Comments.Get;
using Commentium.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Commentium.API.Endpoints
{
    [Route("api/comments")]
    public sealed class CommentsController : Controller
    {
        private ISender _sender;

        public CommentsController(ISender sender) 
        {
            _sender = sender;
        }

        [HttpPost("addcomment")]
        public async Task<IResult> AddComment([FromForm]AddCommentRequest request) 
        {
            var command = new CreateCommentCommand(
                request.UserName,
                request.Email,
                request.Text);
            Result result = await _sender.Send(command);

            return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
        }

        [HttpGet]
        public async Task<IResult> GetComments(
            string? sortColumn,
            string? sortOrder,
            int page = 1,
            int pageSize = 25) 
        {
            var query = new GetCommentsQuery(sortColumn, sortOrder, page, pageSize);
            Result<List<CommentResponse>> result = await _sender.Send(query);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        }
    }
}
