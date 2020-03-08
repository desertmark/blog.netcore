using System.Linq;
using blog.netcore.Models;

public interface IUserRepository {
    IQueryable<User> Get();
    User Get(string UserName);
    User Get(int UserId);
    void Create(User user);
    void Update(User user);
}