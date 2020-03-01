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
        private IUserRepository _userRepository;
        private TokenService _tokenService;

        private User _currentUser = null;
        public User CurrentUser  {
            get {
                if (_currentUser == null) {
                    var session = this._tokenService.GetSession();
                    var userId = int.Parse(session.Identity.Name);
                    this._currentUser = this.Get(userId);
                }
                return this._currentUser;
            }
        }

        public UserService(IUserRepository userRepository, TokenService tokenService) {
            this._userRepository = userRepository;
            this._tokenService = tokenService;
        }

        public IEnumerable<User> Get() {
            return this._userRepository
            .Get()
            .AsEnumerable();
        }

        public User Get(int UserId) {
            return this._userRepository.Get(UserId);
        }
    }
}
