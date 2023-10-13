using Mapster;
using ProtoBuf.Grpc;
using WebApp.Server.Services;
using WebApp.Shared.Grpc;

namespace WebApp.Server.Grpc;

public class UserGrpcService : IUserGrpcService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

    public UserGrpcService(IHttpContextAccessor httpContextAccessor, IUserService userService)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
    }

    public async Task<GetAuthenticatedUserResponse> GetAuthenticatedUser(GetAuthenticatedUserRequest request, CallContext callContext = default)
    {
        var userIdClaim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserId");
        if (userIdClaim == null)
        {
            return new GetAuthenticatedUserResponseNotAuthorized();
        }

        var user = await _userService.GetUserById(Guid.Parse(userIdClaim.Value));
        var response = user.Adapt<GetAuthenticatedUserResponseAuthorized>();
        return response;
    }
}