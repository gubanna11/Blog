using System.Threading.Tasks;

namespace Blog.Infrastructure.Abstract.Interfaces;

public interface IUnitOfWork
{
    IGenericRepository<T> GenericRepository<T>() where T : class;
    Task SaveChangesAsync();
}
