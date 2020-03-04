using System;
using System.Collections.Generic;
using System.Linq;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Services
{
    public class UserService : IUserService
    {
        private IUserRepository userRepository;
        private TokenService tokenService;

        private User currentUser = null;
        public User CurrentUser  {
            get {
                if (currentUser == null) {
                    var session = this.tokenService.GetSession();
                    var userId = int.Parse(session.Identity.Name);
                    this.currentUser = this.Get(userId);
                }
                return this.currentUser;
            }
        }

        public UserService(IUserRepository userRepository, TokenService tokenService) {
            this.userRepository = userRepository;
            this.tokenService = tokenService;
        }

        public IEnumerable<User> Get() {
            return this.userRepository
            .Get()
            .AsEnumerable();
        }

        public User Get(int UserId) {
            return this.userRepository.Get(UserId);
        }

        public User Get(string UserName) {
            return this.userRepository.Get(UserName);
        }
    }
}
