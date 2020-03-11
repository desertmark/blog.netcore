using System.Collections.Generic;
using System.Threading.Tasks;
using blog.netcore.Models;
namespace blog.netcore.Services 
{
    public interface IPostService {
        Task<IEnumerable<Post>> Get();
    }
}