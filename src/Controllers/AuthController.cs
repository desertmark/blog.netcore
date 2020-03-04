using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Models;
using blog.netcore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using blog.netcore.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace blog.netcore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly TokenService _tokenService;
        private readonly ILogger<Post> _logger;

        public AuthController(ILogger<Post> logger, IUserService userService, TokenService tokenService)
        {
            _logger = logger;
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("token")]
        public TokenResponse Post([FromBody] LoginModel model)
        {
            var user = this._userService.Get().First(u => u.UserName == model.UserName);
            var token = this._tokenService.GenerateToken(user);
            return new TokenResponse() {
                Token = token.EncodedToken,
                ExpiresAt = token.TokenModel.ValidTo,
                Sub = token.TokenModel.Subject,
                UniqueName = token.TokenModel.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value
            };
        }
    }
}
