using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService userService;
        private readonly ITokenService tokenService;
        public AuthService(IUserService userService, ITokenService tokenService)
        {
            this.userService = userService;
            this.tokenService = tokenService;
        }

        public async Task<RefreshToken> Login(string username, string password)
        {
            var user = await this.userService.Get(username);
            if (user != null)
            {
                if (user.PasswordHash == this.HashPassword(password, user.Salt))
                {
                    var nonce = this.GenerateSalt();
                    user.Nonce = nonce;
                    await this.userService.Update(user);
                    return new RefreshToken()
                    {
                        Refresh = this.tokenService.GenerateRefreshToken(user),
                        Token = this.tokenService.GenerateToken(user),
                    };
                }
                else
                {
                    throw new LoginException("User name or password are incorrect");
                }
            }
            else
            {
                throw new LoginException("User not found");
            }
        }

        public void LogOut(User user)
        {
            user.Nonce = null;
            this.userService.Update(user);
        }

        public async Task<RefreshToken> RefreshToken(string refreshToken)
        {
            var decodedToken = this.tokenService.DecodeToken(refreshToken);
            var user = await this.userService.Get(int.Parse(decodedToken.Subject));

            var isRefreshToken = this.tokenService.IsRefreshToken(decodedToken);
            var isValidRefreshToken = this.tokenService.ValidateRefreshToken(refreshToken, user);

            if (!isRefreshToken || !isValidRefreshToken)
            {
                throw new LoginException("Invalid Token");
            }
            var newToken = this.tokenService.GenerateToken(user);
            Token newRefresh;
            if (newToken.TokenModel.ValidTo > decodedToken.ValidTo)
            {
                newRefresh = this.tokenService.GenerateRefreshToken(user);
            }
            else
            {
                newRefresh = new Token()
                {
                    EncodedToken = refreshToken,
                    TokenModel = decodedToken,
                };
            }
            return new RefreshToken()
            {
                Refresh = newRefresh,
                Token = newToken,
            };
        }

        public string HashPassword(string password, string salt)
        {
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var bytes = KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8);
            return Convert.ToBase64String(bytes);
        }

        public string GenerateSalt()
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public User CreateUser(string username, string password)
        {
            var salt = this.GenerateSalt();
            var user = new User()
            {
                UserName = username,
                Salt = salt,
                PasswordHash = this.HashPassword(password, salt),
            };
            return user;
        }
    }
}
