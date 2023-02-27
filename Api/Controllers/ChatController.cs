using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ChatController : Controller
    {
        public ChatController()
        {
        }

        public IActionResult ChatPage(CancellationToken cancellationToken)
        {
            string? username = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            return username == null ? Redirect($"{Request.Headers["Origin"]}/Session/Login") : View();
        }

        [HttpPost]
        public Task PostMessage([FromForm] string message, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}