using Blog.Infrastructure.Abstract.Interfaces;
using Blog.Infrastructure.Data;
using System.Threading.Tasks;

namespace Blog.Infrastructure.Abstract;

public sealed class UnitOfWork<T> : IUnitOfWork<T> where T : class
{
    private readonly ApiDataContext _context;
    private readonly IGenericRepository<T> _genericRepository;

    public UnitOfWork(ApiDataContext context, IGenericRepository<T> genericRepository)
    {
        _context = context;
        _genericRepository = genericRepository;
    }

    public IGenericRepository<T> GenericRepository => _genericRepository;

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
