using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Abstract.Interfaces;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Set { get; }
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default,
        params Expression<Func<T, object>>[] includeProperties);

    Task<T?> GetByIdAsync(object id, CancellationToken cancellationToken);
    Task AddAsync(T entity, CancellationToken cancellationToken);
    void Update(T entity);
    Task<T?> RemoveAsync(object id, CancellationToken cancellationToken);
    void RemoveRange(IEnumerable<T> entitities);
}