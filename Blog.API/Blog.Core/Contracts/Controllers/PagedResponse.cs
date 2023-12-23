using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Blog.Core.Contracts.Controllers;

public sealed record PagedResponse<TEntity>(IEnumerable<TEntity> Items, int Page, int PageSize, int TotalCount)
{
    public bool IsNextPage => Page * PageSize < TotalCount;
    public bool IsPreviousPage => Page > 1;

    public static async Task<PagedResponse<TEntity>> CreateAsync(IQueryable<TEntity> query, int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        int totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        return new(items, page, pageSize, totalCount);
    }

    public static async Task<PagedResponse<TEntity>> CreateAsync<TSource>(IQueryable<TSource> query, int page,
        int pageSize,
        Func<IEnumerable<TSource>, IEnumerable<TEntity>> mapFunction,
        Func<List<TSource>, List<TSource>>? additionalFunction,
        CancellationToken cancellationToken = default)
    {
        int totalCount = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        if (additionalFunction is not null)
        {
            items = additionalFunction(items);
        }
        
        IEnumerable<TEntity> mappedItems = mapFunction(items);
        
        return new(mappedItems, page, pageSize, totalCount);
    }
}