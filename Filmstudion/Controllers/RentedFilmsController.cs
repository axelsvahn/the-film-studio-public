using Filmstudion.Data;
using Filmstudion.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Filmstudion.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RentedFilmsController : ControllerBase
    {
        private readonly IRentedFilmRepository _rentedFilmRepository;
        private readonly UserManager<User> _userManager;

        public RentedFilmsController(IRentedFilmRepository rentedfilmrepository, UserManager<User> userManager
        )
        {
            _rentedFilmRepository = rentedfilmrepository;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task <ActionResult<IEnumerable<RentedFilm>>> GetAllRentedFilms()
        {
            try
            {
                var username = User.Identity.Name;

                //filtering based on user takes place in repository
                var result = await _rentedFilmRepository.GetAllRentedFilmsByUserAsync(username);

                {
                    if (result == null) return NotFound("Det fanns inga uthyrda filmer att visa");
                }

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj då! Något gick fel.");
            }
        }
    }
}
