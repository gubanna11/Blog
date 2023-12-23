using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Contracts.Controllers;

public sealed record CursorPagedResponse<T>(IEnumerable<T> Items, Guid? Cursor, int PageSize)
{
    public static async Task<CursorPagedResponse<T>> CreateAsync(IQueryable<T> query, Guid cursor, int pageSize,
        Expression<Func<T, bool>> filter,
        Func<T?, Guid?> nextCursorFunc,
        CancellationToken cancellationToken = default)
    {
        query = query.Where(filter);
        var items = await query.Take(pageSize).ToListAsync(cancellationToken);

        var nextCursor = nextCursorFunc(items.LastOrDefault());
        
        return new(items, nextCursor, pageSize);
    }
}