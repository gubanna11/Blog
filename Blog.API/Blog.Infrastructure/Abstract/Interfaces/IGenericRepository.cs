using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Abstract.Interfaces;

public interface IGenericRepository<T> where T : class
{
    IQueryable<T> Set { get; }
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
    Task<T?> GetByIdAsync(object id);
    Task AddAsync(T entity);
    void Update(T entity);
    T? Remove(object id);
    void RemoveRange(IEnumerable<T> entitities);
}