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
using Microsoft.AspNetCore.Authorization;

namespace blog.netcore.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {

        private readonly IUserService userService;
        private readonly AuthService authService;
        private readonly ILogger<Post> logger;

        public AuthController(ILogger<Post> logger, IUserService userService, AuthService authService)
        {
            this.logger = logger;
            this.userService = userService;
            this.authService = authService;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]
        public IActionResult Post([FromBody] LoginModel model)
        {
            try {
                var refresh = authService.Login(model.UserName, model.Password);
                return Ok(new TokenResponse() {
                    RefreshToken = refresh.Refresh.EncodedToken,
                    Token = refresh.Token.EncodedToken,
                    ExpiresAt = refresh.Token.TokenModel.ValidTo,
                    Sub = refresh.Token.TokenModel.Subject,
                    UniqueName = refresh.Token.TokenModel.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value
                });
            } catch (LoginException error) {
                return Unauthorized(new ErrorResponse() {
                    Message = error.Message
                });
            }
        }
        [HttpDelete]
        [Route("token")]
        public IActionResult Delete() {
            this.authService.LogOut(this.userService.CurrentUser);
            return Ok();
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("refresh")]
        public IActionResult Post([FromBody] RefreshModel model) {
            try {
                var refresh = this.authService.RefreshToken(model.RefreshToken);
                return Ok(new TokenResponse() {
                    RefreshToken = refresh.Refresh.EncodedToken,
                    Token = refresh.Token.EncodedToken,
                    ExpiresAt = refresh.Token.TokenModel.ValidTo,
                    Sub = refresh.Token.TokenModel.Subject,
                    UniqueName = refresh.Token.TokenModel.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.UniqueName)?.Value
                });
            } catch(LoginException error) {
                return Unauthorized(new ErrorResponse() { Message = error.Message });
            }
        }
    }
}
