using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Serilog.Core;


namespace API.Controllers
{
    [ApiController]
    [Route("api/users")] // api/users
    public class UsersController : ControllerBase
    {

        private readonly DataContext _context;

        public UsersController(DataContext context)
        {

            _context = context;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<IEnumerable<AppUser>> GetUSers()
        {
            var users = _context.Users.ToList();

            return users;
            

        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")] // /api/users/id
        public ActionResult<AppUser> GetUser(int id) {
            
            var user = _context.Users.Find(id);
            
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }
    }
}
