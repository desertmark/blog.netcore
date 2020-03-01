using System.Collections.Generic;
using blog.netcore.Models;

public interface IUserService {

    User CurrentUser { get; }
    IEnumerable<User> Get();
    User Get(int UserId);
}