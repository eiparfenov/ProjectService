using Microsoft.AspNetCore.Components.Authorization;

namespace WebApp.Client.Extensions;

public static class AuthenticationStateExtensions
{
    public static string GetFirstName(this AuthenticationState state) =>
        state.User?.Claims.SingleOrDefault(x => x.Type == "FirstName")?.Value ?? "";
    
    public static string GetLastName(this AuthenticationState state) =>
        state.User?.Claims.SingleOrDefault(x => x.Type == "LastName")?.Value ?? "";
}