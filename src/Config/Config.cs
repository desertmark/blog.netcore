using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using blog.netcore.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using blog.netcore.Context;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Configuration
{
    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        private readonly IOptions<AppSettings> appSettings;
        public ConfigureJwtBearerOptions(IOptions<AppSettings> appSettings)
        {
            this.appSettings = appSettings;
        }

        public void PostConfigure(string name, JwtBearerOptions opts)
        {
            //configure with dependency
            opts.RequireHttpsMetadata = false;
            opts.SaveToken = true;
            opts.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKeyResolver = this.GetUserKey,
            };            
        }

        private IEnumerable<SecurityKey> GetUserKey(
            string token, SecurityToken securityToken, 
            string kid,
            TokenValidationParameters validationParameters)
        {
            JwtSecurityToken jwt = (JwtSecurityToken) securityToken;
            var user = this.GetUserById(int.Parse(jwt.Subject));
            var key = this.appSettings.Value.Secret + user.Nonce;
            var keys = new List<SecurityKey>();
            keys.Add(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)));
            return keys;
        }

        private User GetUserById(int UserId) {
            using (var db = new BlogContext(this.appSettings))
            {                
                return db.Users.Find(UserId);
            }
        }
    }
}