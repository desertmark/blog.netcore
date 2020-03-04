using System;
using blog.netcore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace blog.netcore.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {

        private readonly ILogger<Post> _logger;

        public HomeController(ILogger<Post> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new {
              ok = true,
              time = DateTime.Now,
            });
        }
    }
}
