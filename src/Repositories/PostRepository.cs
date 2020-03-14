using System;
using System.Linq;
using blog.netcore.Context;
using blog.netcore.Models;

namespace blog.netcore.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(BlogContext db): base(db) {
        }
    }
}
