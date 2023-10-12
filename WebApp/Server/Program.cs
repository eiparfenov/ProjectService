using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApp.Server;
using WebApp.Server.Authentication.Vk;
using WebApp.Server.Services;
using WebApp.Shared.Authentication.Vk;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddOAuth<VkOptions, VkHandler>(VkDefaults.AuthenticationTheme, vkOptions =>
    {
        builder.Configuration.GetSection("Oauth").GetSection("Vk").Bind(vkOptions);
    });
builder.Services.AddHttpClient<VkHandler>();
builder.Services.AddDbContext<ApplicationDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlite(builder.Configuration.GetConnectionString("DataBase"));
});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


var app = builder.Build();


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

#region Authentication

app.MapGet(VkDefaults.LoginUrl, async context =>
{
    await context.ChallengeAsync(VkDefaults.AuthenticationTheme, new AuthenticationProperties()
    {
        RedirectUri = "/"
    });
});
app.MapGet(VkDefaults.LogoutUrl, async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/");
});

#endregion

app.MapFallbackToFile("index.html");

app.Run();