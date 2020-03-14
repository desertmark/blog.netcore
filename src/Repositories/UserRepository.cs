using System;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(BlogContext db): base(db) {
        }

        public async Task<User> Get(string UserName) {
            return await this.db.Users.FirstOrDefaultAsync(u => u.UserName == UserName);
        }
    }
}
