using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Repositories;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Services
{
    public class CommentService : ICommentService
    {
        private readonly CommentFilter defaultFilters = new CommentFilter();
        private ICommentRepository commentRepository;
        private IPostRepository postRepository;
        private IUserService userService;
        public CommentService(ICommentRepository commentRepository, IPostRepository postRepository, IUserService userService)
        {
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
            this.userService = userService;
        }

        public async Task<IEnumerable<Comment>> Get(int postId, CommentFilter filters = null)
        {
            if (filters == null)
            {
                filters = defaultFilters;
            }
            var currentUser = this.userService.CurrentUser;
            return await this.commentRepository
            .Get()
            .Include("User")
            .Where(comment => comment.Post.PostId == postId)
            .OrderByDescending(comment => comment.CreatedAt)
            .Skip(filters.Page * filters.Size)
            .Take(filters.Size)
            .ToListAsync();
        }

        public async Task<int> Count(int postId)
        {
            return await this.commentRepository.Get()
            .Where(comment => comment.Post.PostId == postId)
            .CountAsync();
        }

        public async Task<Comment> Create(Comment comment)
        {
            comment.CreatedAt = DateTime.Now;
            comment.User = this.userService.CurrentUser;
            return await this.commentRepository.Create(comment);
        }

        public async Task Delete(int commentId)
        {
            var comment = await this.commentRepository.Get(commentId);
            if (comment != null)
            {
                await this.commentRepository.Delete(comment);
            }
            else
            {
                throw new PublicException("Comment not found.");
            }
        }
    }
}
