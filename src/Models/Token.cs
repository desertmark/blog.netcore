using System;
using System.IdentityModel.Tokens.Jwt;

namespace blog.netcore.Models
{
    public class Token
    {
        public string EncodedToken { get; set; }
        public JwtSecurityToken TokenModel { get; set; }

    }

    public class RefreshToken {
        public Token Refresh { get; set; }
        public Token Token { get; set; }
    }
}
