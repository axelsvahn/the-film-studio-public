using Filmstudion.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Data
{
    public interface IFilmRepository
    {
        IEnumerable<Film> AllFilms { get; }
        Film GetFilmByName(string name);
        Film GetFilmById(int id);
        bool AddFilm(Film newfilm);
        bool UpdateFilm(Film updatedfilm);
        bool CheckIfAvailable(Film filmtocheck);
        void ReduceByOne(Film filmtoreduce);
        void IncreaseByOne(Film filmtoincrease);
        bool RemoveFilm(Film filmtoremove);
    }
}
