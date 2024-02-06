using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Blog.Infrastructure.Abstract;

public sealed class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApiDataContext _context;

    public GenericRepository(ApiDataContext context)
    {
        _context = context;
        Set = _context.Set<T>();
    }

    public IQueryable<T> Set { get; }

    public async Task AddAsync(T entity, CancellationToken cancellationToken)
    {
        await _context.Set<T>().AddAsync(entity, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Set<T>().ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken,
        params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _context.Set<T>();

        query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken)
    {
        return await _context.Set<T>().FindAsync(id, cancellationToken);
    }

    public async Task<T?> RemoveAsync(object id, CancellationToken cancellationToken)
    {
        var entity = await _context.Set<T>().FindAsync(id, cancellationToken);
        if (entity is not null)
        {
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Deleted;

            return entity;
        }

        return null;
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        _context.Set<T>().RemoveRange(entities);
    }

    public void Update(T entity)
    {
        _context.Set<T>().Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }
}