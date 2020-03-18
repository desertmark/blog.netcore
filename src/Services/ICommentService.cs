using System.Collections.Generic;
using System.Threading.Tasks;
using blog.netcore.Models;

namespace blog.netcore.Services
{
    public interface ICommentService
    {
        Task<int> Count(int postId);
        Task<Comment> Create(Comment comment);
        Task Delete(int commentId);
        Task<IEnumerable<Comment>> Get(int postId, CommentFilter filters = null);
    }
}