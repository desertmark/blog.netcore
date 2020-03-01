using System.Linq;
using blog.netcore.Models;

public interface IPostRepository {
    IQueryable<Post> Get();
}