using AutoMapper;
using Filmstudion.Data.Entities;
using Filmstudion.Data.Models;

namespace Filmstudion.Data
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Film, FilmModel>();
        }
    }
}

