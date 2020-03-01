using System;
using System.Linq;
using blog.netcore.Context;
using blog.netcore.Models;

namespace blog.netcore.Repositories
{
    public class PostRepository : IPostRepository
    {
        private BlogContext _db;
        public PostRepository(BlogContext db) {
            this._db = db;
        }

        public IQueryable<Post> Get() {
            return this._db.Posts.AsQueryable();
        }
    }
}
