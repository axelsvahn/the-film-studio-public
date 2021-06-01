using Filmstudion.Data;
using Filmstudion.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    [Route("api/v1/filmstudios")] 
    public class StudiosController : ControllerBase
    {
        private readonly IStudioRepository _repository;
        private readonly UserManager<User> _userManager;
        private readonly IRentedFilmRepository _rentedFilmRepository;

        public StudiosController(IStudioRepository repository, UserManager<User> userManager, IRentedFilmRepository rentedfilmrepository)  
        {
            _repository = repository;
            _userManager = userManager;
            _rentedFilmRepository = rentedfilmrepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Studio>> GetAllStudios() //url-parameter api/v1/filmstudios
        {
            try
            {
                IEnumerable<Studio> result = _repository.AllStudios;
                if (result == null)
                {
                    return NotFound("Det fanns inga registrerade filmstudios.");
                }

                return result.ToList();
            }
            catch (Exception)
            {
                throw;
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj då! Något gick fel.");
            }
        }

        [HttpGet("{name}")] //url-parameter api/v1/filmstudios/NAMN
        public ActionResult<Studio> GetSpecificStudioByName(string name)
        {
            try
            {
                var result = _repository.GetStudioByName(name);
                if (result == null)
                {
                    return NotFound("Det fanns ingen filmstudio med detta namn.");
                }
                return result;
            }
            catch (Exception)
            {
                throw;
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj då! Något gick fel.");
            }
        }

        [HttpGet("{id:int}")] //url-parameter api/v1/filmstudios/ID
        public ActionResult<Studio> GetSpecificStudioById(int id)
        {
            try
            {
                var result = _repository.GetStudioById(id);
                if (result == null)
                {
                    return NotFound("Det fanns ingen filmstudio med detta id");
                }
                return result;
            }
            catch (Exception)
            {
                throw;
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj då! Något gick fel.");
            }
        }

        //om man testar denna på seedade studios krävs att man även seedar motsvarande användare också. Annars blir det 
        //expeption pga. användare är null. 

        [HttpGet("{id:int}/rentedfilms")]
        public async Task<ActionResult<IEnumerable<RentedFilm>>> GetRentedFilmsByID(int id)
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(User.Identity.Name);

                if (!user.IsAdmin)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Användare är ej administratör.");
                }

                var studio = _repository.GetStudioById(id);
                if (studio == null)
                {
                    return NotFound("Det fanns ingen filmstudio med detta id");
                }

                var result = await _rentedFilmRepository.GetAllRentedFilmsByUserAsync(studio.PresidentEmail);

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

        [HttpDelete("{id:int}")] //url-parameter api/v1/filmstudios/ID
        public async Task <ActionResult<Studio>> DeleteStudio(int id)
        {
            try
            {
                //auktorisering här för admin via if-statement

                var username = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(username);

                if (!user.IsAdmin)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Användare är ej administratör.");
                }

                var studioToRemove = _repository.GetStudioById(id);
                if (studioToRemove == null)
                {
                    return NotFound("Det fanns ingen filmstudio med detta id");
                }

                bool removeSuccess = _repository.RemoveStudio(studioToRemove); 

                //ska vara idempotent så vi gör inget med boolen. 
                return Ok("Studion finns inte (mer)");
            }

            catch (Exception)
            {
                throw;
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj då! Något gick fel.");
            }
        }
    }
}