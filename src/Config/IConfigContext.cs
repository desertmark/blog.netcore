using Microsoft.EntityFrameworkCore;
using blog.netcore.Models;

namespace blog.netcore.Configuration
{
    public interface IConfigContext
    {
        DbSet<User> Users { get; set; }
    }
}
