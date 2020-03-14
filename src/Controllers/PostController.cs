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
    [Route("[controller]")]
    public class PostController : ControllerBase
    {

        private readonly IPostService postService;
        private readonly ILogger<Post> logger;

        public PostController(ILogger<Post> logger, IPostService postService)
        {
            this.logger = logger;
            this.postService = postService;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] PostFilter filters)
        {
            var items = await this.postService.Get(filters);
            var total = await this.postService.Count();
            return Ok(new PaginatedResponse<Post>() {
                Items = items,
                Total = total,
                Page = filters.Page,
                Size = filters.Size
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            return Ok(await postService.Get(id));
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Post model) {
            var post = await this.postService.Create(model);
            return Created($"/Post/{post.PostId}",post);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int postId) {
            try {
                await this.postService.Delete(postId);
                return NoContent();
            } catch(PublicException error) {
                return BadRequest(new ErrorResponse() { Message = error.Message });
            }
        }
    }
}
