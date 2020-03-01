using System.Linq;
using blog.netcore.Models;

public interface IUserRepository {
    IQueryable<User> Get();
    User Get(int UserId);
}