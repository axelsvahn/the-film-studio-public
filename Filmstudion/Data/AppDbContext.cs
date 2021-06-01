using Filmstudion.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Filmstudion.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            this.Database.EnsureCreated();
        }

        public DbSet<Film> Films { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<RentedFilm> RentedFilms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seed Studios
            modelBuilder.Entity<Studio>().HasData(new Studio
            {
                StudioId = 1,
                Name = "Uddebos filmstudio",
                Location = "Uddebogatan 1, Uddebo",
                PresidentName = "Viktor Lyresten",
                PresidentEmail = "viktor@uddfs.se",
                PresidentPhoneNumber = 123012345,
            }) ;
            modelBuilder.Entity<Studio>().HasData(new Studio
            {
                StudioId = 2,
                Name = "Lunds filmstudio",
                Location = "Lundgatan 1, Lund",
                PresidentName = "Axel Svahn",
                PresidentEmail = "axel@lundfs.se",
                PresidentPhoneNumber = 923012789,
            });

            //seed Films
            modelBuilder.Entity<Film>().HasData(new Film
            {
                FilmId = 1,
                Name = "The Net",
                Country = "USA",
                Director = "Irwin Winkler",
                ReleaseYear = 1995,
                CopiesForRent = 5
            });
            modelBuilder.Entity<Film>().HasData(new Film
            {
                FilmId = 2,
                Name = "Pelle Breaks the Internet",
                Country = "Sverige",
                Director = "Pelle Svensson",
                ReleaseYear = 2021,
                CopiesForRent = 4
            });
            modelBuilder.Entity<Film>().HasData(new Film
            {
                FilmId = 3,
                Name = "You've Got Mail",
                Country = "USA",
                Director = "Nora Ephron",
                ReleaseYear = 1998,
                CopiesForRent = 2
            });
            modelBuilder.Entity<Film>().HasData(new Film
            {
                FilmId = 4,
                Name = "TRON",
                Country = "USA",
                Director = "Steven Lisberger",
                ReleaseYear = 1982,
                CopiesForRent = 3
            });
            modelBuilder.Entity<Film>().HasData(new Film
            {
                FilmId = 5,
                Name = "API-gutterne",
                Country = "Norge",
                Director = "Ole Websen",
                ReleaseYear = 2010,
                CopiesForRent = 1
            });

            //seed rented films
            modelBuilder.Entity<RentedFilm>().HasData(new RentedFilm
            {
                RentedFilmId = 1,
                SourceFilmId = 1,
                SourceFilmName = "The Net",
                RentingStudioId = 1,
                RentingStudioName = "Uddebos filmstudio",
                RentingStudioEmail = "viktor@uddfs.se"
            });
            modelBuilder.Entity<RentedFilm>().HasData(new RentedFilm
            {
                RentedFilmId = 2,
                SourceFilmId = 2,
                SourceFilmName = "Pelle Breaks the Internet",
                RentingStudioId = 3,
                RentingStudioName = "Teststads filmstudio",
                RentingStudioEmail = "test@sff.se"
            });      
        }
    }
}