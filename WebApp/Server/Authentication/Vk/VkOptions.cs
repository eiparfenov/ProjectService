using Microsoft.AspNetCore.Authentication.OAuth;
using WebApp.Shared.Authentication.Vk;

namespace WebApp.Server.Authentication.Vk;

public class VkOptions: OAuthOptions
{
    public VkOptions()
    {
        CallbackPath = new PathString("/oauth/vk-cb");

        TokenEndpoint = VkDefaults.TokenEndpoint;
        AuthorizationEndpoint = VkDefaults.AuthorizationEndpoint;
    }
}