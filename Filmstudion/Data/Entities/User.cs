 using System;
 using System.Collections.Generic;
 using System.Linq;
 using System.Text;
 using System.Threading.Tasks;
 using Microsoft.AspNetCore.Identity;

 namespace Filmstudion.Data.Entities
{
    public class User : IdentityUser //klassen måste heta User och inte ApiUser, annars kan den inte ingå
        // i andra klasser som Studio via composition
    {
        //public int ApiUserId { get; set; } //används inte

        //public string UserName { get; set; } base class (IdentityUser)

        //public string Email { get; set; }  base class (IdentityUser)
        public string Password { get; set; } //visar här i plaintext av debug-skäl
        public bool IsAdmin { get; set; }
    }
}
