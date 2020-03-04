using System;
using System.Collections.Generic;
using System.Linq;
using blog.netcore.Context;
using blog.netcore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace blog.netcore.Services
{
  public class AuthService
  {
    private readonly IUserService userService;
    private readonly TokenService tokenService;
    public AuthService(IUserService userService, TokenService tokenService)
    {
      this.userService = userService;
      this.tokenService = tokenService;
    }

    public Token Login(string username, string password) {
      var user = this.userService.Get(username);
      if (user != null) {
        if (user.PasswordHash == this.hashPassword(password)) {
          return this.tokenService.GenerateToken(user);
        } else {
          throw new LoginException("User name or password are iccorrect");
        }
      } else {
        throw new LoginException("User not found");
      }
    }

    public string hashPassword(string password) {
      return "";
    }

  }
}
