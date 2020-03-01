using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace blog.netcore.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {

        private readonly IPostService _postService;
        private readonly ILogger<Post> _logger;

        public PostController(ILogger<Post> logger, IPostService postService)
        {
            _logger = logger;
            _postService = postService;
        }

        [HttpGet]
        public IEnumerable<Post> Get()
        {
            return _postService.Get();
        }
    }
}
