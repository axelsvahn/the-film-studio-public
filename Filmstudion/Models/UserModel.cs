using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmstudion.Models
{
    public class UserModel
    {
        //used for registration and authentication 

        [Required] 
        [EmailAddress]
        public string Email { get; set; }

        [Required] 
        public string Password { get; set; }

        // bool isAdmin sätts till "true"/"false" direkt i ApiUserRepo.
    }
}

