using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Areas.Authorization.Models;
using WebApplication1.Areas.Home.Models;

namespace WebApplication1.Areas.Authorization.Controllers
{
    [Area("Authorization")]
    public class AuthController : Controller
    {

        private readonly UserStore userStore;

        public AuthController(UserStore userStore)
        {
            this.userStore = userStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/Authorization/Auth/Index")
        {
            var properties = new AuthenticationProperties { RedirectUri = returnUrl };
            return Challenge(properties, "Discord");
        }

        [HttpGet("auth/callback")]
        public async Task<IActionResult> Callback()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync("Discord");
            if (!authenticateResult.Succeeded)
                return BadRequest();

            var claimsIdentity = new ClaimsIdentity((IEnumerable<Claim>?)authenticateResult.Principal.Identity, "Discord");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            var discordId = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var username = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;
            var avatar = claimsPrincipal.FindFirst("urn:discord:avatar")?.Value;

            if (!string.IsNullOrEmpty(discordId))
            {
                var user = new User
                {
                    DiscordId = discordId,
                    Username = username,
                    AvatarUrl = $"https://cdn.discordapp.com/avatars/{discordId}/{avatar}.png"
                };
                userStore.AddUser(user);
            }

            return RedirectToAction("Index", "Auth", new { area = "Authorization" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home", new { area = "Home" });
        }
    }
}