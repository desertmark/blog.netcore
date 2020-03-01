using System;
using System.Linq;
using blog.netcore.Context;
using blog.netcore.Models;

namespace blog.netcore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private BlogContext _db;
        public UserRepository(BlogContext db) {
            this._db = db;
        }

        public IQueryable<User> Get() {
            return this._db.Users.AsQueryable();
        }
        public User Get(int UserId) {
            return this._db.Users.First(u => u.UserId == UserId);
        }
    }
}
