using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace API.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            if (await userManager.Users.AnyAsync()) return; // If there are any users in the database, return
            var userData = await File.ReadAllTextAsync("Migrations/UserSeedData.json");

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            var roles = new List<AppRole>
            {
                new AppRole {Name = "Member"},
                new AppRole {Name = "Admin"},
                new AppRole {Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role); 
            }

            foreach (var user in users)
            {
                user.UserName = user.UserName.ToLower();

                await userManager.CreateAsync(user, "Pa55w@rd"); // Create and save

                await userManager.AddToRoleAsync(user, "Member");
            }

            var admin = new AppUser
            {
                UserName = "admin"
            };

            await userManager.CreateAsync(admin, "Pa55w@rd");

            IEnumerable<string> capabilities = new List<string> { "Admin", "Moderator" };
            
            await userManager.AddToRolesAsync(admin, capabilities);
        }
    }
}
