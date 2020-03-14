using System.Collections.Generic;
using System.Threading.Tasks;
using blog.netcore.Models;
namespace blog.netcore.Services 
{
    public interface IPostService {
        Task<int> Count();
        Task<Post> Create(Post post);
        Task<Post> Get(int postId);
        Task<IEnumerable<Post>> Get(PostFilter filters);
        Task Delete(int postId);
    }
}