using System.Text.Json.Serialization;

namespace Commentium.Application.Comments.Get
{
    public class PagedList<T>
    {
        private PagedList(
            List<T> items,
            int page,
            int pageSize,
            int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        private PagedList() { }

        public List<T> Items { get; private set; }

        public int Page { get; private set; }

        public int PageSize { get; private set; }

        public int TotalCount { get; private set; }

        public bool HasNextPage => Page * PageSize < TotalCount;

        public bool HasPreviousPage => Page > 1;

        public static PagedList<T> Create(List<T> query, int page, int pageSize) 
        {
            var totalCount = query.Count();
            var items = query.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new(items, page, pageSize, totalCount);
        }
    }
}
