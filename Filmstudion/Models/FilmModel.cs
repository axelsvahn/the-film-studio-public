using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data.Models
{
    public class FilmModel
    {
        //visas för icke-autentiserade användare, saknar information om tillgängliga kopior
        public int FilmId { get; set; } 

        public string Name { get; set; }

        public string Country { get; set; }

        public string Director { get; set; }

        public int ReleaseYear { get; set; }
        
    }
}
