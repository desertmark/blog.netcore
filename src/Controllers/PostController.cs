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
        public async Task<IActionResult> Get()
        {
            return Ok(await postService.Get());
        }
    }
}
