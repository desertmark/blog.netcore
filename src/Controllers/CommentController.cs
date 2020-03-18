using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Models;
using blog.netcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace blog.netcore.Controllers
{
    [ApiController]
    [Authorize(Policy = "AccessToken")]
    [Route("/Post/{postId}/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly ICommentService commentService;
        private readonly ILogger<Post> logger;

        public CommentController(ILogger<Post> logger, ICommentService commentService)
        {
            this.logger = logger;
            this.commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromRoute] int postId,[FromQuery] CommentFilter filters)
        {
            var items = await this.commentService.Get(postId, filters);
            var total = await this.commentService.Count(postId);
            return Ok(new PaginatedResponse<Comment>() {
                Total = total,
                Page = filters.Page,
                Size = filters.Size,
                Items = items
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Comment model) {
            var comment = await this.commentService.Create(model);
            return Created($"/Comment/{comment.CommentId}", comment);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int commentId) {
            try {
                await this.commentService.Delete(commentId);
                return NoContent();
            } catch(PublicException error) {
                return BadRequest(new ErrorResponse() { Message = error.Message });
            }
        }
    }
}
