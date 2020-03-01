using System.Collections.Generic;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Context
{
    public class BlogContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Server=localhost;Database=blog;User Id=sa;Password=Abcd1234;");
    }
}