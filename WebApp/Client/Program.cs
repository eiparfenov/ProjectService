using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ProtoBuf.Grpc.ClientFactory;
using WebApp.Client;
using WebApp.Client.Authentication;
using WebApp.Shared.Grpc;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WebApp.ServerAPI",
    client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));

builder.Services.AddAntDesign();
var baseUri = new Uri(builder.HostEnvironment.BaseAddress);
var handler = new GrpcWebHandler(GrpcWebMode.GrpcWebText, new HttpClientHandler());
builder.Services.AddLogging();

builder.Services.AddCodeFirstGrpcClient<IUserGrpcService>(options => options.Address = baseUri).ConfigureChannel(options => options.HttpHandler = handler);
builder.Services.AddCodeFirstGrpcClient<IDepartmentGrpcService>(options => options.Address = baseUri).ConfigureChannel(options => options.HttpHandler = handler);
builder.Services.AddCodeFirstGrpcClient<IEquipmentGrpcService>(options => options.Address = baseUri).ConfigureChannel(options => options.HttpHandler = handler);
builder.Services.AddCodeFirstGrpcClient<IWorkplaceGrpcService>(options => options.Address = baseUri).ConfigureChannel(options => options.HttpHandler = handler);

builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();