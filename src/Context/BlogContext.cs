using System.Collections.Generic;
using blog.netcore.Configuration;
using blog.netcore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace blog.netcore.Context
{
    public class BlogContext : DbContext
    {
        private readonly string connectionString;
        public BlogContext(IOptions<AppSettings> appSettings) {
            this.connectionString = appSettings.Value.ConnectionString;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Post> Posts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(this.connectionString);
    }

    public class BlogContextTransient : BlogContext {
        public BlogContextTransient(IOptions<AppSettings> appSettings) : base(appSettings) {}
    }
}