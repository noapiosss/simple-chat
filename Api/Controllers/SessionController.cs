using System.Security.Claims;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class SessionController : Controller
    {
        private readonly IUserHandler _userHandler;
        public SessionController(IUserHandler userHandler)
        {
            _userHandler = userHandler;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromForm] string username)
        {
            if (_userHandler.UsernameIsAlreadyInUse(username))
            {
                return Redirect($"{Request.Headers["Origin"]}/Session/Login");
            }

            Claim[] claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
            AuthenticationProperties authProperties = new();

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                claimsPrincipal,
                authProperties);

            return Redirect($"{Request.Headers["Origin"]}/Chat/ChatPage");
        }

        public async Task<IActionResult> Logout()
        {
            string username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect($"{Request.Headers["Origin"]}/Session/Login");
        }

        [HttpGet("username")]
        public string? GetSessionusername()
        {
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
        }
    }
}