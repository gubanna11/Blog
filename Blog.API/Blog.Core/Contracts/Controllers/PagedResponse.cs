using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Contracts.Controllers;

public sealed record PagedResponse<T>(IEnumerable<T> Items, int Page, int PageSize, int TotalCount)
{
    public bool IsNextPage => Page * PageSize < TotalCount;
    public bool IsPreviousPage => Page > 1;

    public static async Task<PagedResponse<T>> CreateAsync(IQueryable<T> query, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        int totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new(items, page, pageSize, totalCount);
    }
}