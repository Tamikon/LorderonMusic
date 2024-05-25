using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using WebApplication1.Areas.Authorization.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using WebApplication1.Areas.Home.Models;

namespace WebApplication1
{
    public class DiscordAuthenticationHandler
    {
        public static void ConfigureOAuth(OAuthOptions options, IServiceProvider serviceProvider)
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            options.ClientId = configuration["Discord:ClientId"];
            options.ClientSecret = configuration["Discord:ClientSecret"];
            options.CallbackPath = new PathString("/auth/callback");

            options.AuthorizationEndpoint = "https://discord.com/api/oauth2/authorize";
            options.TokenEndpoint = "https://discord.com/api/oauth2/token";
            options.UserInformationEndpoint = "https://discord.com/api/users/@me";

            options.Scope.Add("identify");
            options.Scope.Add("guilds");

            options.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username");
            options.ClaimActions.MapJsonKey("urn:discord:avatar", "avatar");

            options.Events = new OAuthEvents
            {
                OnCreatingTicket = async context =>
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, options.UserInformationEndpoint);
                    request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);

                    var response = await context.Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, context.HttpContext.RequestAborted);
                    response.EnsureSuccessStatusCode();

                    var user = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                    context.RunClaimActions(user.RootElement);

                    var userStore = context.HttpContext.RequestServices.GetRequiredService<UserStore>();

                    var userId = user.RootElement.GetString("id");
                    var username = user.RootElement.GetString("username");
                    var avatar = user.RootElement.GetString("avatar");
                    var guilds = await GetUserGuildsAsync(context.AccessToken, context.Backchannel, context.HttpContext.RequestAborted);

                    var existingUser = userStore.GetUser(userId);
                    if (existingUser == null)
                    {
                        userStore.AddUser(new User
                        {
                            DiscordId = userId,
                            Username = username,
                            AvatarUrl = avatar,
                            FirstAuthorizationDate = DateTime.UtcNow,
                            Guilds = guilds
                        });
                    }
                    else
                    {
                        existingUser.Username = username;
                        existingUser.AvatarUrl = avatar;
                        existingUser.Guilds = guilds;
                    }
                }
            };
        }

        private static async Task<List<string>> GetUserGuildsAsync(string accessToken, HttpClient backchannel, CancellationToken cancellationToken)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "https://discord.com/api/users/@me/guilds");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            var guilds = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var guildIds = guilds.RootElement.EnumerateArray().Select(g => g.GetProperty("id").GetString()).ToList();

            return guildIds;
        }
    }
}