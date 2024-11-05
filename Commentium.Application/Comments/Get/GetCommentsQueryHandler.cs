using Commentium.Domain.Comments;
using Commentium.Domain.Shared;
using Commentium.Domain.Users;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Commentium.Application.Comments.Get
{
    internal sealed class GetCommentsQueryHandler
        : IRequestHandler<GetCommentsQuery, Result<PagedList<CommentResponse>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;

        public GetCommentsQueryHandler(IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _userRepository = userRepository;
            _commentRepository = commentRepository;
        }

        public async Task<Result<PagedList<CommentResponse>>> Handle(GetCommentsQuery request, CancellationToken cancellationToken)
        {
            var commentsQuery = await GetCommentResponsesAsync();

            if (request.SortOrder?.ToLower() == "desc") 
            {
                commentsQuery = commentsQuery.OrderByDescending(GetSortProperty(request)).ToList();
            }
            else 
            {
                commentsQuery = commentsQuery.OrderBy(GetSortProperty(request)).ToList();
            }

            var comments = PagedList<CommentResponse>.Create(commentsQuery, request.Page, request.PageSize);

            return comments;
        }

        private async Task<List<CommentResponse>> GetCommentResponsesAsync(Guid? parentCommentId = null) 
        {
            var comments = await _commentRepository.GetComments()
                .Where(p => p.ParentCommentId == parentCommentId)
                .Include(u => u.User)
                .Include(t => t.AttachedFile)
                .ToListAsync();

            var commentResponse = comments.Select(c => new CommentResponse(
                c.Id,
                c.User.UserName,
                c.User.Email,
                c.Text,
                c.AttachedFile != null ? c.AttachedFile : null,
                c.CreatedDate,
                GetCommentResponsesAsync(c.Id).Result)).ToList();

            return commentResponse;
        }

        private Func<CommentResponse, object> GetSortProperty(GetCommentsQuery request) =>
            request.SortColumn?.ToLower() switch
            {
                "username" => comment => comment.UserName,
                "email" => comment => comment.UserEmail,
                _ => comment => comment.CreatedDate
            };
    }
}
