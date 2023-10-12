using Hex.Paths;

namespace WebApp.Shared.Authentication.Vk;

public static class VkDefaults
{
    public const string AuthenticationTheme = "Vk";

    public static readonly PathString AuthorizationEndpoint = new PathString("https://oauth.vk.com/authorize");
    public static readonly PathString TokenEndpoint = new PathString("https://oauth.vk.com/access_token");

    public static readonly PathString LoginUrl = new PathString("/vk/login");
    public static readonly PathString LogoutUrl = new PathString("/vk/logout");

    public static readonly string UserDataRequestTemplate = "https://api.vk.com/method/users.get?access_token={0}&v=5.131";
}