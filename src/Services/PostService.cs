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
        private ICommentRepository commentRepository;
        private IPostRepository postRepository;
        private IUserService userService;
        public PostService(IPostRepository postRepository, IUserService userService, ICommentRepository commentRepository) {
            this.postRepository = postRepository;
            this.userService = userService;
            this.commentRepository = commentRepository;
        }

        public async Task<IEnumerable<Post>> Get(PostFilter filters = null) {
            if (filters == null) {
                filters = this.defaultFilters;
            }

            return await Task.Run(async () => {
                var currentUser = this.userService.CurrentUser;
                var posts = await this.postRepository
                .Get()
                .Where(post => post.User.UserId == currentUser.UserId)
                .Skip(filters.Page * filters.Size)
                .Take(filters.Size)
                .Include("User")
                .ToListAsync();

                posts = await this.LoadRecentComments(posts);

                return posts.AsEnumerable();
            });
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

        // loads takes a post list and loads the comments porperty with the last recent posts of each post.
        private async Task<List<Post>> LoadRecentComments(List<Post> posts) {
            var commentsByPost = await this.GetRecentCommentsByPosts(posts);
            for (int i = 0; i < posts.Count; i++)
            {
                posts[i].Comments = commentsByPost[i];
            }
            return posts;
        } 

        // iterates throught a post list and gets the last recent post of each post.
        private async Task<ICollection<Comment>[]> GetRecentCommentsByPosts(List<Post> posts) {
            var taskList = new List<Task<ICollection<Comment>>>();
            posts.ForEach(post => taskList.Add(this.GetRecentComments(post.PostId)));
            return await Task.WhenAll(taskList);
        }

        // loads the last recet comments of a post by the given post id
        private async Task<ICollection<Comment>> GetRecentComments(int postId) {
            return (ICollection<Comment>) await this.commentRepository.GetLastRecent(postId, 2);
        }
    }
}
