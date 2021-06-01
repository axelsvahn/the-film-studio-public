using Filmstudion.Data;
using Filmstudion.Data.Entities;
using Filmstudion.Models;
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
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IApiUserRepository _apiUserRepository;
        private readonly UserManager<User> _userManager;
        private readonly IStudioRepository _studioRepository;

        public RegisterController(IStudioRepository studiorepository, IApiUserRepository apiUserRepository, UserManager<User> userManager)
        {
            _studioRepository = studiorepository;
            _apiUserRepository = apiUserRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("admin")] // url: api/v1/register/admin
        public async Task<IActionResult> RegisterAdmin([FromBody] UserModel model)
        {
            try
            {
                var newAdmin = _apiUserRepository.CreateAdmin(model);
                var result = await _userManager.CreateAsync(newAdmin, model.Password);

                if (result != IdentityResult.Success)
                {
                    return BadRequest("Det gick inte att registrera en användare/studio");
                }
                else
                {
                    return Created("", model);
                }
            }
            catch
            {
                // om något gick fel
                return BadRequest("Det gick inte att registrera en admin");
            }
        }


        [HttpPost("filmstudio")] // url: api/v1/register/filmstudio
        public async Task <IActionResult> RegisterStudio([FromBody] RegisterStudioModel model)
        {
            try
            {
                var newUser = _apiUserRepository.CreateApiUser(model);
                var newStudio = _studioRepository.CreateStudio(model);
                newStudio.User = newUser;

                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result != IdentityResult.Success)
                {
                    return BadRequest("Det gick inte att registrera en användare");
                }

                bool addStudioSuccess = _studioRepository.AddStudio(newStudio);

                var retrievedStudio = _studioRepository.GetStudioByPresidentEmail(newUser.Email);

                if (addStudioSuccess)
                {
                    return Created($"/api/v1/filmstudios/{newStudio.StudioId}", retrievedStudio); 
                }
            }
            catch
            {
                // om något gick fel
                return BadRequest("Det gick inte att registrera en studio");
            }
            return BadRequest("Det gick inte att registrera en studio");
        }
    }
}
