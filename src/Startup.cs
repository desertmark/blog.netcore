using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Context;
using blog.netcore.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using blog.netcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using blog.netcore.Models;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using blog.netcore.Configuration;

namespace blog.netcore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //NetCore
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // Settings
            var settings = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(settings);
            // Context
            services.AddDbContext<BlogContext>();
            // Post
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostService, PostService>();

            var appSettings = settings.Get<AppSettings>();
            // AUTH
            services.AddScoped<TokenService>();
            services.AddScoped<AuthService>();
            // User
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserService, UserService>();

            services.AddSingleton<IPostConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>();
            services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer();
            services.AddAuthorization(opts => {
                opts.AddPolicy("AccessToken", policy => {
                    policy.RequireAssertion(context => {
                        return context.User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Typ)?.Value == "access";
                    });
                });
            });
            services.AddControllers();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IUserService userService, AuthService authService)
        {
            if (env.IsDevelopment())
            {
                this.CreateAdminUser(userService, authService);
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors();
        }

        public void CreateAdminUser(IUserService userService, AuthService authService) {
            if (userService.Get().Count() == 0) {
                var admin = authService.CreateUser("admin", "admin");
                userService.Create(admin);
            }
        }
    }
}
