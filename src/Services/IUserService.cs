using System.Collections.Generic;
using System.Threading.Tasks;
using blog.netcore.Models;

public interface IUserService {

    User CurrentUser { get; }
    Task<IEnumerable<User>> Get();
    Task<User> Get(int UserId);
    Task<User> Get(string UserName);
    Task Create(User user);
    Task Update(User user);
}