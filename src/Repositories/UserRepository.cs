using System;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Repositories
{
    public class UserRepository : IUserRepository
    {
        private BlogContext db;
        public UserRepository(BlogContext db) {
            this.db = db;
        }

        public IQueryable<User> Get() {
            return this.db.Users.AsQueryable();
        }

        public async Task<User> Get(int UserId) {
            return await this.db.Users.FirstOrDefaultAsync(u => u.UserId == UserId);
        }

        public async Task<User> Get(string UserName) {
            return await this.db.Users.FirstOrDefaultAsync(u => u.UserName == UserName);
        }

        public async Task Create(User user) {
            await this.db.AddAsync(user);
            await this.db.SaveChangesAsync();
        }

        public async Task Update(User user) {
            this.db.Users.Update(user);
            await this.db.SaveChangesAsync();
        }
    }
}
