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
using AutoMapper;
using Filmstudion.Data.Models;

namespace Filmstudion.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class FilmsController : ControllerBase
    {
        private readonly IFilmRepository _filmRepository;
        private readonly IStudioRepository _studioRepository;
        private readonly IRentedFilmRepository _rentedFilmRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public FilmsController(IFilmRepository filmrepository, IStudioRepository studiorepository, IRentedFilmRepository rentedfilmrepository, UserManager<User> userManager, IMapper mapper)
        {
            _filmRepository = filmrepository;
            _studioRepository = studiorepository;
            _rentedFilmRepository = rentedfilmrepository;
            _userManager = userManager;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet] //url: api/v1/films/
        public ActionResult<IEnumerable<Film>> GetAllFilms()
        {
            try
            {
                IEnumerable<Film> result = _filmRepository.AllFilms;
                {
                    if (result == null) return NotFound("Det fanns inga filmer att visa");
                }

                var username = User.Identity.Name;

                if (username != null)
                {
                    return result.ToList();
                }
                else if (username == null) //if not authenticated: returns model without copies for rent
                {
                    return Ok(_mapper.Map<IEnumerable<Film>, IEnumerable<FilmModel>>(result));
                }
                else //if not authenticated
                {
                    return Ok(_mapper.Map<IEnumerable<Film>, IEnumerable<FilmModel>>(result));
                }
            }
            catch (Exception)
            {
                throw;
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj då! Något gick fel.");
            }
        }

        [AllowAnonymous]
        [HttpGet("{id:int}")] //url: api/v1/films/ID
        public ActionResult<Film> GetSpecificFilmById(int id)
        {
            try
            {
                var result = _filmRepository.GetFilmById(id);
                if (result == null)
                {
                    return NotFound("Det fanns ingen film med detta id.");
                }

                var username = User.Identity.Name;

                if (username != null)
                {
                    return result;
                }
                else if (username == null) //if not authenticated
                {
                    return Ok(_mapper.Map<Film, FilmModel>(result));
                }
                else //if not authenticated
                {
                    return Ok(_mapper.Map<Film, FilmModel>(result));
                }
            }
            catch (Exception)
            {
                throw;
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj då! Något gick fel.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<Film>> PostNewFilm(Film newfilm) //url: api/v1/films/
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

                var duplicateName = _filmRepository.GetFilmByName(newfilm.Name);
                if (duplicateName != null) //checks if film with that name already exists in database
                {
                    return BadRequest("Det finns redan en film med det namnet.");
                }

                var duplicateId = _filmRepository.GetFilmById(newfilm.FilmId);
                if (duplicateId != null) //checks if film with that Id already exists in database
                {
                    return BadRequest("Det finns redan en film med detta id.");
                }

                //add film and check result

                bool addSuccess = _filmRepository.AddFilm(newfilm);

                if (addSuccess)
                {
                    //if successfully entered into database
                    //cannot use Ok(), post method requires different return value
                    return Created($"/api/v1/films/{newfilm.FilmId}", newfilm);
                }

            }
            catch (Exception)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "database failure");
                throw;
            }

            //if bad thing happended that did not cause exception, i.e. did not save to database properly
            return BadRequest("Det gick inte att spara filmen. Var god kontrollera indata.");
        }

        [HttpPut("{filmid}")] //url: api/v1/films/ID
        public async Task<ActionResult<Film>> UpdateExistingFilm(int filmid, Film updatedfilm)
        {
            try
            {
                //auktorisering här för admin via if-statement
                var username = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(username);

                if (!user.IsAdmin)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden, "Användare är ej administratör.");
                }

                //auktorisering här för admin

                var oldFilm = _filmRepository.GetFilmById(filmid);
                if (oldFilm == null) //checks if film with that id exists in database
                {
                    return NotFound($"Det finns ingen film med id {filmid} att uppdatera.");
                }

                _filmRepository.UpdateFilm(updatedfilm);

                //put should be idempotent, so no error if database not updated due to
                //content being identical with preexisting content

                return _filmRepository.GetFilmByName(updatedfilm.Name);

            }
            catch (Exception)
            {
                return BadRequest("Det gick inte att uppdatera filmen. Var god kontrollera indata.");
            }
        }

        [HttpPost("{filmid}/rent")] //url: api/v1/films/ID/rent
        public async Task<ActionResult<RentedFilm>> RentFilm(int filmid)
        {
            try
            {
                //auktorisering av vanliga användare
                var username = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(username);

                if (user.IsAdmin)
                {
                    return StatusCode(StatusCodes.Status403Forbidden, "Administratörer får inte låna filmer.");
                }

                if (user == null) //vid fel i autentiseringen som kommer förbi [Authorize]
                {
                    return BadRequest("Det finns ingen autentiserad användare.");
                }

                //Skaffa filmen / kolla om den existerar: 
                var filmToRent = _filmRepository.GetFilmById(filmid);
                if (filmToRent == null) //checks if film with that id exists in database
                {
                    return NotFound($"Det finns ingen film med id {filmid} att hyra.");
                }

                //Skaffa studion / kolla om den existerar: 
                var rentingStudio = _studioRepository.GetStudioByPresidentEmail(username);
                if (rentingStudio == null) //
                {
                    return BadRequest("Det finns ingen filmstudio knuten till användaren.");
                }

                //Kolla om studion redan lånat filmen och därmed inte kan låna den nu
                bool haveRented = _rentedFilmRepository.CheckIfRented(filmid, rentingStudio.StudioId);
                if (haveRented == true) //
                {
                    return BadRequest("Studion har redan lånat ett exemplar av filmen.");
                }

                // Kolla om det finns utlåningsbara exemplar av filmen 
                bool isAvailable = _filmRepository.CheckIfAvailable(filmToRent);
                if (isAvailable == false) //
                {
                    return BadRequest("Alla exemplar av filmen är utlånade.");
                }

                //Skapa själva RentedFilm-objektet 
                var newRentedFilm = _rentedFilmRepository.CreateRentedFilm(filmToRent, rentingStudio);
                if (newRentedFilm == null) //Om något blir fel med skapandet av objektet
                {
                    return BadRequest("Filmen är tillgänglig att hyra, men hyrningen misslyckades. Var god kontrollera indata.");
                }

                bool addSuccess = _rentedFilmRepository.AddRentedFilm(newRentedFilm);
                if (addSuccess)
                {
                    //if successfully entered into database, reduce number by one
                    _filmRepository.ReduceByOne(filmToRent);

                    //cannot use Ok(), post method requires different return value
                    return Created($"/api/v1/rentedfilms/", newRentedFilm);
                }
            }
            catch (Exception)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj! Något blev fel i servern.");
                throw;
            }
            //hanterar övrigt
            return BadRequest("Det gick inte att hyra filmen. Var god kontrollera indata.");
        }

        [HttpPut("{filmid}/return")] //url: api/v1/films/ID/return
        public async Task<ActionResult<Film>> ReturnFilm(int filmid)
        {
            try
            {
                var username = User.Identity.Name;
                var user = await _userManager.FindByNameAsync(username);

                if (user.IsAdmin)
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden, "Administratörer får inte låna filmer.");
                }

                //Skaffa filmen / kolla om den existerar: 
                var filmToReturn = _filmRepository.GetFilmById(filmid);
                if (filmToReturn == null) //checks if film with that id exists in database
                {
                    return NotFound($"Det finns ingen film med id {filmid}.");
                }

                //Skaffa studion / kolla om den existerar: 
                var rentingStudio = _studioRepository.GetStudioByPresidentEmail(username);
                if (rentingStudio == null)
                {
                    return BadRequest("Det finns ingen filmstudio knuten till användaren.");
                }

                //Checks if film is rented by studio. Same message as on successful return in order to make it idempotent.
                bool isRented = _rentedFilmRepository.CheckIfRented(filmid, rentingStudio.StudioId);
                if (!isRented)
                {
                    return Ok(($"'{filmToReturn.Name}' är inte (eller inte längre) hyrd av studion."));
                }

                //if we have gotten this far in the logic, we can remove the rented film
                bool removeSuccess = _rentedFilmRepository.RemoveRentedFilm(filmid, rentingStudio.StudioId);
                if (removeSuccess)
                {
                    //if successfully removed from database, increase rentable copies by one
                    _filmRepository.IncreaseByOne(filmToReturn);

                    return Ok(($"'{filmToReturn.Name}' är inte (eller inte längre) hyrd av studion."));
                }
            }
            catch (Exception)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj! Något blev fel i servern.");
                throw;
            }
            //hanterar övrigt
            return BadRequest("Det gick inte att lämna tillbaka filmen. Var god kontrollera indata.");
        }

        [HttpDelete("{id:int}")] //url-parameter api/v1/films/ID
        public async Task<ActionResult<Studio>> DeleteFilm(int id)
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

                var filmToRemove = _filmRepository.GetFilmById(id);
                if (filmToRemove == null)
                {
                    return NotFound("Det fanns ingen film med detta id");
                }

                bool removeSuccess = _filmRepository.RemoveFilm(filmToRemove);

                //ska vara idempotent så vi gör inget med boolen. 
                return Ok("Filmen finns inte (mer)");
            }

            catch (Exception)
            {
                //return this.StatusCode(StatusCodes.Status500InternalServerError, "Oj! Något blev fel i servern.");
                throw;
            }

        }
    }
}
