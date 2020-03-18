using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Repositories
{
    public class CommentRepository : BaseRepository<Comment>, ICommentRepository
    {
        public CommentRepository(IServiceProvider provider): base(provider) {
        }
        public async Task<IEnumerable<Comment>> GetLastRecent(int postId, int take)
        {
            return await this.dbTransient.Comments
            .Include("Post")
            .Include("User")
            .Where(comment => comment.Post.PostId == postId)
            .OrderByDescending(comment => comment.CreatedAt)
            .Take(take)
            .ToListAsync();
        }

    }
}
