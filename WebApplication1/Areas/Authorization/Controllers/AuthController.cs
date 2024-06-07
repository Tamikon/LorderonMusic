using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Areas.Authorization.Models;
using DatabaseService.Models;

namespace WebApplication1.Areas.Authorization.Controllers
{
    [Area("Authorization")]
    public class AuthController : Controller
    {
        private readonly UserStore _userStore;

        public AuthController(UserStore userStore)
        {
            _userStore = userStore;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("login")]
        public IActionResult Login(string returnUrl = "/Home/Home/Index")
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
                var avatarUrl = !string.IsNullOrEmpty(avatar)
                    ? $"https://cdn.discordapp.com/avatars/{discordId}/{avatar}.png"
                    : "https://cdn.discordapp.com/embed/avatars/0.png";

                var user = new User
                {
                    DiscordId = discordId,
                    Username = username,
                    AvatarUrl = avatarUrl,
                    Servers = new List<Server>()
                };

                await _userStore.AddOrUpdateUser(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }
    }
}
