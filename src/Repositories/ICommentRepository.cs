using System.Collections.Generic;
using System.Threading.Tasks;
using blog.netcore.Models;
namespace blog.netcore.Repositories {
    public interface ICommentRepository : IBaseRepository<Comment> {
        Task<IEnumerable<Comment>> GetLastRecent(int postId, int take);
    }
}