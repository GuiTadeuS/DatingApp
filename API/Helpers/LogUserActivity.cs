using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var afterAPIAction = await next();

            if (!afterAPIAction.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = afterAPIAction.HttpContext.User.GetUserId();

            var repo = afterAPIAction.HttpContext.RequestServices.GetService<IUserRepository>(); 

            var user  = await repo.GetUserByIdAsync(userId); //int.Parse

            user.LastActive = DateTime.UtcNow;

            await repo.SaveAllAsync();

        }
    }
}
