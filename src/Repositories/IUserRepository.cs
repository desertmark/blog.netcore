using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Models;
namespace blog.netcore.Repositories {
    public interface IUserRepository : IBaseRepository<User> {
        Task<User> Get(string UserName);
    }
}