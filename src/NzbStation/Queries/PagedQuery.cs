using NzbStation.Models;
using Zynapse;

namespace NzbStation.Queries
{
    public class PagedQuery<T> : IQuery<PagedResultModel<T>>
    {
        public int? Page { get; set; }

        public int? Size { get; set; }
    }
}
