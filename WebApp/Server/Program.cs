using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;
using WebApp.Server;
using WebApp.Server.Authentication.Vk;
using WebApp.Server.Grpc;
using WebApp.Server.Services;
using WebApp.Server.Services.Initialization;
using WebApp.Shared.Authentication.Vk;
using WebApp.Shared.Grpc;

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
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("DataBase"));
});

builder.Services.AddHostedService<MigrateDb<ApplicationDbContext>>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddCodeFirstGrpc();

var app = builder.Build();


app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseGrpcWeb();

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
app.MapGet("/auth-test", async context =>
{
    var user = context.User.Claims.Select(x => $"{x.Type} {x.Value}");
    await context.Response.WriteAsync(string.Join(", ", user));
});

#endregion

#region Grpc

app.MapGrpcService<UserGrpcService>().EnableGrpcWeb();
app.MapGrpcService<DepartmentGrpcService>().EnableGrpcWeb();

#endregion

app.MapFallbackToFile("index.html");

app.Run();