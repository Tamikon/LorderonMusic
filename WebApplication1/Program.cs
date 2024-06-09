using DatabaseService;
using DatabaseService.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Areas.Authorization.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.Configure(builder.Configuration.GetSection("Kestrel"));
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IMusicRepository, MusicRepository>();
builder.Services.AddHttpClient();
builder.Services.AddSingleton(provider =>
    new YouTubeService(
        provider.GetRequiredService<IHttpClientFactory>(),
        "AIzaSyA2TxFlqiE5wW2zfQkjC-_LmKkXfDHPxMk"));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = "Discord";
}).AddCookie()
.AddOAuth("Discord", options => DiscordAuthenticationHandler.ConfigureOAuth(options, builder.Services.BuildServiceProvider()));

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<UserStore>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
//pattern: "{area=Authorization}/{controller=Auth}/{action=Index}/{id?}");
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}");

app.Run();