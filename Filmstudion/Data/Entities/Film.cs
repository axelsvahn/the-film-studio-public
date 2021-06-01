using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data.Entities
{
    public class Film
    {
        [Required] //denna första bör dock kunna automatgenereras
        public int FilmId { get; set; } //behövs eventuellt för att få EF Core att fungera
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string Country { get; set; }
        [Required]
        [StringLength(75)]
        public string Director { get; set; }
        [Required]
        [Range(1850,2022)]
        public int ReleaseYear { get; set; }
        [Required]
        public int CopiesForRent { get; set; }
    }
}
