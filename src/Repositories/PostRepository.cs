using System;
using System.Linq;
using blog.netcore.Context;
using blog.netcore.Models;

namespace blog.netcore.Repositories
{
    public class PostRepository : IPostRepository
    {
        private BlogContext db;
        public PostRepository(BlogContext db) {
            this.db = db;
        }

        public IQueryable<Post> Get() {
            return this.db.Posts.AsQueryable();
        }
    }
}
