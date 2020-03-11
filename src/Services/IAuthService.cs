using System.Threading.Tasks;
using blog.netcore.Models;

namespace blog.netcore.Services
{
    public interface IAuthService
    {
        User CreateUser(string username, string password);
        string GenerateSalt();
        string HashPassword(string password, string salt);
        Task<RefreshToken> Login(string username, string password);
        void LogOut(User user);
        Task<RefreshToken> RefreshToken(string refreshToken);
    }
}