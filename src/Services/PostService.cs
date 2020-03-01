using System;
using System.Collections.Generic;
using System.Linq;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Repositories
{
    public class PostService : IPostService
    {
        private IPostRepository _postRepository;
        private IUserService _userService;
        public PostService(IPostRepository postRepository, IUserService userService) {
            this._postRepository = postRepository;
            this._userService = userService;
        }

        public IEnumerable<Post> Get() {
            var currentUser = this._userService.CurrentUser;
            return this._postRepository
            .Get()
            .Include("User")
            .Where(post => post.User.UserId == currentUser.UserId)
            .AsEnumerable();
        }
    }
}
