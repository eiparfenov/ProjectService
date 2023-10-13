using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Shared.Grpc;

namespace WebApp.Client.Authentication;

public class CookieAuthenticationStateProvider: AuthenticationStateProvider
{
    private readonly IUserGrpcService _userGrpcService;
    private readonly ILogger<CookieAuthenticationStateProvider> _logger;
    
    public CookieAuthenticationStateProvider(IUserGrpcService userGrpcService, ILogger<CookieAuthenticationStateProvider> logger)
    {
        _userGrpcService = userGrpcService;
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _logger.LogWarning("Попытка аворизации");
        var response = await _userGrpcService.GetAuthenticatedUser(new GetAuthenticatedUserRequest()) as GetAuthenticatedUserResponseAuthorized;

        if (response == null)
        {
            _logger.LogWarning("Провал");

            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new ("FirstName", response.FirstName!),
            new ("LastName", response.LastName!)
        }, "AuthCookie"));
        _logger.LogWarning("Успех");

        return new AuthenticationState(principal);
    }
}