using Filmstudion.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public class FilmRepository : IFilmRepository
    {
        private readonly AppDbContext _appDbContext;
        public FilmRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Film> AllFilms
        {
            get
            {
                return _appDbContext.Films;
            }
        }

        public Film GetFilmByName(string name)
        {
            IQueryable<Film> result = _appDbContext.Films
              .Where(f => f.Name == name);

            return result.FirstOrDefault();
        }

        public Film GetFilmById(int id)
        {
            IQueryable<Film> result = _appDbContext.Films
              .Where(f => f.FilmId == id);

            return result.FirstOrDefault();
        }

        public bool AddFilm(Film newfilm)
        {
            _appDbContext.Films.Add(newfilm);

            //bool representerar om något ändrats i appDbContext. 
            return (_appDbContext.SaveChanges()) > 0;
        }

        public bool UpdateFilm(Film updatedfilm)
        {
            foreach (Film f in AllFilms)
                if (f.FilmId == updatedfilm.FilmId)
                {
                    f.Name = updatedfilm.Name;
                    f.Country = updatedfilm.Country;
                    f.Director = updatedfilm.Director;
                    f.ReleaseYear = updatedfilm.ReleaseYear;
                    f.CopiesForRent = updatedfilm.CopiesForRent;
                    break;
                }
            //bool representerar om något ändrats i appDbContext. 
            return (_appDbContext.SaveChanges()) > 0;
        }

        public bool CheckIfAvailable(Film filmtocheck)
        {
            //Kollar om filmens kopior överstiger 0.
            if (filmtocheck.CopiesForRent > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void ReduceByOne(Film filmtoreduce)
        {
            foreach (Film f in AllFilms)
                if (f.FilmId == filmtoreduce.FilmId)
                {
                    f.CopiesForRent -= 1;
                    break;
                }
            _appDbContext.SaveChanges();
        }

        public void IncreaseByOne(Film filmtoincrease)
        {
            foreach (Film f in AllFilms)
                if (f.FilmId == filmtoincrease.FilmId)
                {
                    f.CopiesForRent += 1;
                    break;
                }
            _appDbContext.SaveChanges();
        }

        public bool RemoveFilm(Film filmtoremove)
        {
            _appDbContext.Films.Remove(filmtoremove);
            //bool representerar om något ändrats i appDbContext. 
            return (_appDbContext.SaveChanges()) > 0;
        }
    }
}
