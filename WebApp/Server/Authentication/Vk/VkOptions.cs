using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OAuth;
using WebApp.Shared.Authentication.Vk;

namespace WebApp.Server.Authentication.Vk;

public class VkOptions: OAuthOptions
{
    public VkOptions()
    {
        CallbackPath = new PathString("/oauth/vk-cb");
        SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        TokenEndpoint = VkDefaults.TokenEndpoint;
        AuthorizationEndpoint = VkDefaults.AuthorizationEndpoint;
        
        ClaimActions.MapJsonKey("UserId", "UserId");
    }
}