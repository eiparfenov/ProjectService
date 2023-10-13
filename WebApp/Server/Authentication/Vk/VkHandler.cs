using System.Net;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using WebApp.Server.Services;
using WebApp.Shared.Authentication.Vk;

namespace WebApp.Server.Authentication.Vk;

public class VkHandler: OAuthHandler<VkOptions>
{
    private readonly HttpClient _httpClient;
    private readonly IUserService _userService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public VkHandler(IOptionsMonitor<VkOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, HttpClient httpClient, IUserService userService, IHttpContextAccessor httpContextAccessor) : base(options, logger, encoder, clock)
    {
        _httpClient = httpClient;
        _userService = userService;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
    {
        
        Logger.LogInformation("Got user token {token}", tokens.AccessToken);
        using var httpResponse = await _httpClient.GetAsync(string.Format(VkDefaults.UserDataRequestTemplate, tokens.AccessToken));
        
        var response = await httpResponse.Content.ReadFromJsonAsync<RootResponse>();
        var userData = response!.Response.First();

        var userId = await _userService.GetUserOnLoginByVk(userData.Id, userData.FirstName!, userData.LastName!);
        var jsonUser = JsonSerializer.SerializeToElement($"{{\"UserId\": {userId}}}");

        var principal = new ClaimsPrincipal(identity);
        var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme,
            Options, Backchannel, tokens, jsonUser);
        identity.AddClaim(new("UserId", userId.ToString()!));
        await Events.CreatingTicket(context);
        return new AuthenticationTicket(principal, context.Properties, Scheme.Name);
    }
    
    public class UserResponse
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("first_name")]
        public string? FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string? LastName { get; set; }

        [JsonPropertyName("can_access_closed")]
        public bool CanAccessClosed { get; set; }

        [JsonPropertyName("is_closed")]
        public bool IsClosed { get; set; }
    }

    public class RootResponse
    {
        [JsonPropertyName("response")]
        public List<UserResponse> Response { get; set; }
    }
}