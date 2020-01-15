using System.Collections.Generic;

namespace NzbStation.Models
{
    public class PagedResultModel<T>
    {
        public PagedResultModel(IReadOnlyCollection<T> items, int pageNumber, int pageCount, int totalItemCount)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalItemCount = totalItemCount;
            PageCount = pageCount;
        }

        public IReadOnlyCollection<T> Items { get; }

        public int PageNumber { get; }

        public int PageCount { get; }

        public int TotalItemCount { get; }

        public int ItemCount => Items.Count;

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < PageCount;
    }
}
