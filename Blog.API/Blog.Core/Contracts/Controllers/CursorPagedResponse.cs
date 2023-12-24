using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Contracts.Controllers;

public sealed record CursorPagedResponse<TEntity>(IEnumerable<TEntity> Items, Guid? Cursor, int PageSize)
{
    public static async Task<CursorPagedResponse<TEntity>> CreateAsync(IQueryable<TEntity> query, Guid cursor, int pageSize,
        Expression<Func<TEntity, bool>> cursorFilter,
        Func<TEntity?, Guid?> nextCursorFunc,
        CancellationToken cancellationToken = default)
    {
        query = query.Where(cursorFilter);
        var items = await query.Take(pageSize).ToListAsync(cancellationToken);

        var nextCursor = nextCursorFunc(items.LastOrDefault());
        
        return new(items, nextCursor, pageSize);
    }
    
    public static async Task<CursorPagedResponse<TEntity>> CreateAsync<TSource>(IQueryable<TSource> query, Guid cursor, int pageSize,
        Expression<Func<TSource, bool>> cursorFilter,
        Func<TSource?, Guid?> nextCursorFunc,
        Func<IEnumerable<TSource>, IEnumerable<TEntity>> mapFunction,
        Func<List<TSource>, List<TSource>>? additionalFunction,
        CancellationToken cancellationToken = default)
    {
        query = query.Where(cursorFilter);
        var items = await query.Take(pageSize).ToListAsync(cancellationToken);
        
        if (additionalFunction is not null)
        {
            items = additionalFunction(items);
        }

        var nextCursor = nextCursorFunc(items.LastOrDefault());
        
        IEnumerable<TEntity> mappedItems = mapFunction(items);
        
        return new(mappedItems, nextCursor, pageSize);
    }
}