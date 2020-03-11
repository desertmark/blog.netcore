using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Services
{
    public class PostService : IPostService
    {
        private IPostRepository postRepository;
        private IUserService userService;
        public PostService(IPostRepository postRepository, IUserService userService) {
            this.postRepository = postRepository;
            this.userService = userService;
        }

        public async Task<IEnumerable<Post>> Get() {
            var currentUser = this.userService.CurrentUser;
            return await this.postRepository
            .Get()
            .Include("User")
            .Where(post => post.User.UserId == currentUser.UserId)
            .ToListAsync();
        }
    }
}
