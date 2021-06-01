using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data.Entities
{
    public class RentedFilm
    {
        [Required]
        public int RentedFilmId { get; set; }

        [Required]
        public int SourceFilmId { get; set; }

        [Required]
        public string SourceFilmName { get; set; }

        [Required]
        public int RentingStudioId { get; set; }

        [Required]
        public string RentingStudioName { get; set; }

        [Required]
        public string RentingStudioEmail { get; set; } //link to user

    }
}
