using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Repositories;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Services
{
    public class PostService : IPostService
    {
        private readonly PostFilter defaultFilters = new PostFilter();
        private IPostRepository postRepository;
        private IUserService userService;
        public PostService(IPostRepository postRepository, IUserService userService) {
            this.postRepository = postRepository;
            this.userService = userService;
        }

        public async Task<IEnumerable<Post>> Get(PostFilter filters = null) {
            if (filters == null) {
                filters = this.defaultFilters;
            }
            var currentUser = this.userService.CurrentUser;
            return await this.postRepository
            .Get()
            .Include("User")
            .Where(post => post.User.UserId == currentUser.UserId)
            .Skip(filters.Page * filters.Size)
            .Take(filters.Size)
            .ToListAsync();
        }

        public async Task<int> Count() {
            return await this.postRepository.Count();
        }

        public async Task<Post> Create(Post post) {
            post.CreatedAt = DateTime.Now;
            post.User = this.userService.CurrentUser;
            return await this.postRepository.Create(post);
        }

        public async Task Delete(int postId) {
            var post = await this.postRepository.Get(postId);
            if (post != null) {
                await this.postRepository.Delete(post);
            } else {
                throw new PublicException("Post not found.");
            }
        }

        public async Task<Post> Get(int postId)
        {
            return await this.postRepository.Get(postId);
        }
    }
}
