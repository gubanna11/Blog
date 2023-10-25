using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Data;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Abstract;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ApiDataContext _context;

    public UnitOfWork(ApiDataContext context)
    {
        _context = context;
    }

    public IGenericRepository<T> GenericRepository<T>() where T : class
    {
        return new GenericRepository<T>(_context);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
