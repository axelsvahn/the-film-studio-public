using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filmstudion.Models
{
    public class RegisterStudioModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        public string PresidentName { get; set; }

        [Required]
        [EmailAddress]
        public string PresidentEmail { get; set; } //Studio kopplas till användare via denna
        [Required]
        public int PresidentPhoneNumber { get; set; }
    }
}

