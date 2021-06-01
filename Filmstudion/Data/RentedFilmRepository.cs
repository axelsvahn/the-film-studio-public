using Filmstudion.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public class RentedFilmRepository : IRentedFilmRepository
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;

        public RentedFilmRepository(AppDbContext appDbContext, UserManager<User> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
        }

        public IEnumerable<RentedFilm> AllRentedFilms
        {
            get
            {
                return _appDbContext.RentedFilms;
            }
        }

        public async Task<IEnumerable<RentedFilm>> GetAllRentedFilmsByUserAsync(string username)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user.IsAdmin) //ger alla uthyrda filmer
                {
                    return _appDbContext.RentedFilms.ToList();
                }
                else if (!user.IsAdmin) //ger endast egna uthyrda filmer
                {
                    return _appDbContext.RentedFilms
                   .Where(f => f.RentingStudioEmail == username)
                   .ToList();
                }
            }
            catch
            {
                throw new Exception();
            }
            throw new Exception();
        }

        public bool CheckIfRented(int filmid, int studioid)
        {
            //checks if incoming parameters match a RentedFilm in the database, meaning 
            //a certain film has already been rented by a certain studio

            bool hasRented = false;

            foreach (RentedFilm r in AllRentedFilms)
                if (r.SourceFilmId == filmid && r.RentingStudioId == studioid)
                {
                    hasRented = true;
                    break;
                }
            return hasRented;
        }

        public RentedFilm CreateRentedFilm(Film originfilm, Studio rentingstudio)
        {
            //manually maps from film/studio to RentedFilm

            RentedFilm newRentedFilm = new RentedFilm();

            newRentedFilm.SourceFilmId = originfilm.FilmId;
            newRentedFilm.SourceFilmName = originfilm.Name;
            newRentedFilm.RentingStudioId = rentingstudio.StudioId;
            newRentedFilm.RentingStudioName = rentingstudio.Name;
            newRentedFilm.RentingStudioEmail = rentingstudio.PresidentEmail;

            return newRentedFilm;
        }

        public bool AddRentedFilm(RentedFilm rentedfilm)
        {
            _appDbContext.RentedFilms.Add(rentedfilm);

            //bool representerar om något ändrats i appDbContext. 
            return (_appDbContext.SaveChanges()) > 0;
        }

        public bool RemoveRentedFilm(int filmid, int studioid)
        {
            foreach (RentedFilm r in AllRentedFilms)
                if (r.SourceFilmId == filmid && r.RentingStudioId == studioid)
                {
                    _appDbContext.RentedFilms.Remove(r);
                    break;
                }

            //bool representerar om något ändrats i appDbContext. 
            return (_appDbContext.SaveChanges()) > 0;
        }
    }
}