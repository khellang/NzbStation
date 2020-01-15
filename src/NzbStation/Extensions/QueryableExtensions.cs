using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NzbStation.Models;
using NzbStation.Queries;

namespace NzbStation.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResultModel<T>> AsPagedAsync<T>(this IQueryable<T> source, PagedQuery<T> query, CancellationToken cancellationToken)
        {
            var rowCount = await source.CountAsync(cancellationToken);

            var page = query.Page ?? 1;
            var size = query.Size ?? 30;

            var pageSize = Math.Clamp(size, min: 1, max: 200);

            var pageCount = (rowCount - 1) / pageSize + 1;
            var pageNumber = Math.Clamp(page, min: 1, max: pageCount);

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return new PagedResultModel<T>(items, pageNumber, pageCount, rowCount);
        }
    }
}
