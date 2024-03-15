using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Serilog.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;


namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {

        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {

            _userRepository = userRepository;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUSers()
        {
            return Ok( await _userRepository.GetUsersAsync());
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{username}")] // /api/users/username
        public async Task<ActionResult<AppUser>> GetUser(string username)
        {

            return await _userRepository.GetUserByUsernameAsync(username);
        }

    }
}
