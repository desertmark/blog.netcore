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
    public class TokenService : ITokenService
    {
        private HttpContext context;
        private ILogger logger;
        private JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        private AppSettings appSettings;

        public TokenService(IOptions<AppSettings> appSettings, IHttpContextAccessor contextAccessor, ILogger<Token> logger) {
            this.context = contextAccessor.HttpContext;
            this.appSettings = appSettings.Value;
            this.logger = logger;
        }
        public JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public ClaimsPrincipal GetSession()
        {
            return this.context?.User;
        }

        public SymmetricSecurityKey GenerateUserKey(User user) {
            var key = this.appSettings.Secret + user.Nonce;
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key));
        }

        public bool ValidateRefreshToken(string refreshToken, User user) {
            var handler = new JwtSecurityTokenHandler();
            var validations = new TokenValidationParameters() {
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = this.GenerateUserKey(user),
                ClockSkew = TimeSpan.Zero,
            };
            SecurityToken validToken;
            try {
                var principal = handler.ValidateToken(refreshToken, validations, out validToken);
                return true;
            } catch {
                return false;
            }
        }

        public Token GenerateToken(User user) {
            // token type access: valid to access api resources
            var claims = this.GetClaims(user).Append(new Claim(JwtRegisteredClaimNames.Typ, "access"));
            return this.GenerateToken(user, DateTime.Now.AddSeconds(3600), claims);
        }

        public Token GenerateRefreshToken(User user) {
            // token type refresh: only valid in refresh token endpoint
            var claims = this.GetClaims(user).Append(new Claim(JwtRegisteredClaimNames.Typ, "refresh"));
            return this.GenerateToken(user, DateTime.Now.AddSeconds(7200), claims);
        }

        private Token GenerateToken(User user, DateTime expires, IEnumerable<Claim> claims)
        {
            var key = this.GenerateUserKey(user);
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var descriptor = new SecurityTokenDescriptor() {
                Subject = new ClaimsIdentity(claims),
                Expires = expires,
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

        public bool IsRefreshToken(JwtSecurityToken token) {
            return token.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Typ)?.Value == "refresh";
        }
    }
}
