using System.Linq;
using System.Threading.Tasks;

namespace blog.netcore.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Create(T entity);
        Task Delete(T entity);
        IQueryable<T> Get();
        Task<T> Get(int id);
        Task<int> Count();
        Task Update(T entity);
    }
}