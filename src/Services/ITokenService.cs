using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using blog.netcore.Models;
using Microsoft.IdentityModel.Tokens;

namespace blog.netcore.Services
{
    public interface ITokenService
    {
        JwtSecurityToken DecodeToken(string token);
        Token GenerateRefreshToken(User user);
        Token GenerateToken(User user);
        SymmetricSecurityKey GenerateUserKey(User user);
        ClaimsPrincipal GetSession();
        bool IsRefreshToken(JwtSecurityToken token);
        bool ValidateRefreshToken(string refreshToken, User user);
    }
}