using System.Threading.Tasks;

namespace Blog.Infrastructure.Abstract.Interfaces;

public interface IUnitOfWork<T> where T : class
{
    IGenericRepository<T> GenericRepository<T> { get; }
    Task SaveChangesAsync();
}
