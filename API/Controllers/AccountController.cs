using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController: BaseApiController
    {
        private readonly DataContext _context;

        public AccountController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")] // api/account/register
        public async Task<ActionResult<AppUser>> Register(string username, string password)
        {
            using var hmac = new HMACSHA512(); // password salt

            var user = new AppUser
            {
                UserName = username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)), // password hash
                PasswordSalt = hmac.Key                                            // password salt randonly generated
            };

            _context.Users.Add(user);                                              // add user to context
            await _context.SaveChangesAsync();                                     // save changes to db
            return user;
        }
    }
}
