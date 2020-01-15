using System.Collections.Generic;

namespace NzbStation.Models
{
    public class PagedResultModel<T>
    {
        public int Page { get; set; }

        public int TotalPages { get; set; }

        public IReadOnlyCollection<T> Results { get; set; }

        public int TotalResults { get; set; }
    }
}
