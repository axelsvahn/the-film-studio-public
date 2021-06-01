using Filmstudion.Data.Entities;
using Filmstudion.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public class ApiUserRepository : IApiUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public ApiUserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public User CreateApiUser(RegisterStudioModel model)
        {
            var user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                Password = model.Password,
                IsAdmin = false
            };
            return user;
        }

        public User CreateAdmin(UserModel model)
        {
            var user = new User()
            {
                Email = model.Email,
                UserName = model.Email,
                Password = model.Password,
                IsAdmin = true
            };
            return user;
        }
    }
}


