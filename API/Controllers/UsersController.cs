using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Serilog.Core;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers
{
    public class UsersController : BaseApiController
    {

        private readonly DataContext _context;

        public UsersController(DataContext context)
        {

            _context = context;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUSers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
            

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")] // /api/users/id
        public async Task<ActionResult<AppUser>> GetUser(int id) {
            
            var user = await _context.Users.FindAsync(id);
            
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
    }
}
