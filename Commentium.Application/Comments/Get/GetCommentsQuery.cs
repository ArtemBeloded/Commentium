using Commentium.Domain.Shared;
using MediatR;

namespace Commentium.Application.Comments.Get
{
    public record GetCommentsQuery(
        string? SortColumn,
        string? SortOrder,
        int Page,
        int PageSize) : IRequest<Result<PagedList<CommentResponse>>>;
}
