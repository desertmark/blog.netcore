using System.Collections.Generic;
using blog.netcore.Models;

public interface IUserService {

    User CurrentUser { get; }
    IEnumerable<User> Get();
    User Get(int UserId);
    User Get(string UserName);
    void Create(User user);
    void Update(User user);
}