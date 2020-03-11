using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Models;

public interface IUserRepository {
    IQueryable<User> Get();
    Task<User> Get(string UserName);
    Task<User> Get(int UserId);
    Task Create(User user);
    Task Update(User user);
}