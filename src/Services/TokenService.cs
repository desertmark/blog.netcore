using System;
using System.Linq;
using blog.netcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Security.Principal;
using System.Collections.Generic;
using Microsoft.Extensions.Options;

namespace blog.netcore.Services
{
    public class TokenService
    {
        private HttpContext _context;
        private ILogger _logger;
        private JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        private AppSettings _appSettings;

        public TokenService(IOptions<AppSettings> appSettings, IHttpContextAccessor contextAccessor, ILogger<Token> logger) {
            _context = contextAccessor.HttpContext;
            _appSettings = appSettings.Value;
            _logger = logger;
        }
        private JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public ClaimsPrincipal GetSession()
        {
            return this._context.User;
        }

        // public User Authenticate(string userName, string password) {

        // }

        public Token GenerateToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(this._appSettings.Secret);
            var claims = this.GetClaims(user);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);
            var descriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(1),
                SigningCredentials = credentials,
            };
            var token = this.handler.CreateJwtSecurityToken(descriptor);
            return new Token() {
                EncodedToken = this.handler.WriteToken(token),
                TokenModel = token,
            };
        }

        private Claim[] GetClaims(User user)
        {
            return new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };
        }
    }
}
