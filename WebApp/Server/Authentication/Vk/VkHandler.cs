using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Options;
using WebApp.Server.Services;
using WebApp.Shared.Authentication.Vk;

namespace WebApp.Server.Authentication.Vk;

public class VkHandler: OAuthHandler<VkOptions>
{
    private readonly HttpClient httpClient;
    private readonly IUserService _userService;
    
    public VkHandler(IOptionsMonitor<VkOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, HttpClient httpClient, IUserService userService) : base(options, logger, encoder, clock)
    {
        this.httpClient = httpClient;
        _userService = userService;
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
    {
        
        Logger.LogInformation("Got user token {token}", tokens.AccessToken);
        using var httpResponse = await httpClient.GetAsync(string.Format(VkDefaults.UserDataRequestTemplate, tokens.AccessToken));
        
        var response = await httpResponse.Content.ReadFromJsonAsync<RootResponse>();
        var userData = response!.Response.First();

        var userId = _userService.GetUserOnLoginByVk(userData.Id, userData.FirstName!, userData.LastName!);
        
        return await base.CreateTicketAsync(identity, properties, tokens);
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