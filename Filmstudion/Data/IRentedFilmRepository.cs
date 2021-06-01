using Filmstudion.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public interface IRentedFilmRepository
    {
        IEnumerable<RentedFilm> AllRentedFilms { get; }
        Task<IEnumerable<RentedFilm>> GetAllRentedFilmsByUserAsync(string username);
        bool CheckIfRented(int filmid, int studioid);
        RentedFilm CreateRentedFilm(Film originfilm, Studio rentingstudio);
        bool AddRentedFilm(RentedFilm rentedfilm);
        bool RemoveRentedFilm(int filmid, int studioid);
    }
}
