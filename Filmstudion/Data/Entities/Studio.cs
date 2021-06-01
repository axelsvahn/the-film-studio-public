using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data.Entities
{
    public class Studio 
    {
        //validering sköts i RegisterStudioModel istället
        public int StudioId { get; set; } 
        public string Name { get; set; }
        public string Location { get; set; }

        public string PresidentName { get; set; }
        public string PresidentEmail { get; set; } //link to user
        public int PresidentPhoneNumber { get; set; }

        public User User { get; set; } //klassen måste heta user, annars fungerar detta inte 

    }
}
