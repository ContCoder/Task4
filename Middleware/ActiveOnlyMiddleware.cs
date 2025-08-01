using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Task4.AppDbContext;
using System.Security.Claims;

namespace Task4.Middleware
{
    public class ActiveOnlyMiddleware
    {
        private readonly RequestDelegate _next;

        public ActiveOnlyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Context dbContext)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path != null && (
                path.StartsWith("/login") ||
                path.StartsWith("/signin") ||
                path.StartsWith("/css") ||
                path.StartsWith("/js") ||
                path.StartsWith("/lib") ||
                path.StartsWith("/images")
            ))
            {
                await _next(context);
                return;
            }

            if (!context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }

            var email = context.User.FindFirst(ClaimTypes.Email)?.Value;
            var usuario = dbContext.Users.FirstOrDefault(u => u.Email == email && u.isActive);

            if (usuario == null)
            {
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme, new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow,
                    IsPersistent = false,
                    AllowRefresh = false
                });

                context.Response.Redirect("/Login/Index");
                return;
            }

            await _next(context);
        }


    }
}
