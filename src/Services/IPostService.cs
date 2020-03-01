using System.Collections.Generic;
using blog.netcore.Models;

public interface IPostService {
    IEnumerable<Post> Get();
}