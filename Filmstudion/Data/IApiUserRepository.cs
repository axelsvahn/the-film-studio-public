using Filmstudion.Data.Entities;
using Filmstudion.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public interface IApiUserRepository
    {
        public User CreateApiUser(RegisterStudioModel model);
        public User CreateAdmin(UserModel model);
    }
}
