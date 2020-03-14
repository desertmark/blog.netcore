using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using blog.netcore.Repositories;
using blog.netcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private ITokenService tokenService;
        public User CurrentUser { get; set; }

        public UserService(IUserRepository userRepository, ITokenService tokenService)
        {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
            this.InitCurrentUser();
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await this.userRepository.Get().ToListAsync();
        }

        public async Task<User> Get(int UserId)
        {
            return await this.userRepository.Get(UserId);
        }

        public async Task<User> Get(string UserName)
        {
            return await this.userRepository.Get(UserName);
        }

        public async Task Create(User user)
        {
            await this.userRepository.Create(user);
        }

        public async Task Update(User user)
        {
            await this.userRepository.Update(user);
        }

        private void InitCurrentUser() {
            var session = this.tokenService.GetSession();
            if (session != null) {
                var userName = session.Identity.Name;
                this.CurrentUser = this.Get(userName).Result;
            }
        }
    }
}
