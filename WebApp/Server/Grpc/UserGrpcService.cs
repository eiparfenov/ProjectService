using AntDesign;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Models;
using ProtoBuf.Grpc;
using WebApp.Server.Services;
using WebApp.Shared;
using WebApp.Shared.Grpc;

namespace WebApp.Server.Grpc;

public class UserGrpcService : IUserGrpcService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<UserGrpcService> _logger;
    private HttpContext HttpContext => _httpContextAccessor.HttpContext!;

    public UserGrpcService(IHttpContextAccessor httpContextAccessor, IUserService userService, ApplicationDbContext db, ILogger<UserGrpcService> logger)
    {
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _db = db;
        _logger = logger;
    }

    public async Task<GetAuthenticatedUserResponse> GetAuthenticatedUser(GetAuthenticatedUserRequest request)
    {
        var userIdClaim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserId");
        if (userIdClaim == null)
        {
            return new GetAuthenticatedUserResponseNotAuthorized();
        }

        var user = await _userService.GetUserById(Guid.Parse(userIdClaim.Value));
        var roles = await _db.UserRoles
            .AsNoTracking()
            .Where(userRole => userRole.UserId == user.Id)
            .Include(userRole => userRole.Role)
            .ThenInclude(role => role!.MatchedDepartment)
            .Select(userRole => userRole.Role!)
            .ToListAsync();

        var response = user.Adapt<GetAuthenticatedUserResponseAuthorized>();
        response.Roles = FormRoles(roles, includeSimpleRoles: true).ToList();
        return response;
    }

    public async Task<GetAllRolesInDepartmentResponse> GetAllRolesInDepartment(GetAllRolesInDepartmentRequest request)
    {
        return new GetAllRolesInDepartmentResponse()
        {
            Roles = new[] { RoleType.Receptionist, RoleType.Employee, RoleType.AdminDepartment }
                .Select(x => Enum.GetName(typeof(RoleType), x)!)
                .ToList()
        };
    }

    public async Task<PerformUserAdminPanelResponse> PerformUserAdminPanel(PerformUserAdminPanelRequest request)
    {
        var department = await _db.Departments.SingleAsync(dep => dep.UrlTitle == request.DepartmentUrl);
        if (request.UsersToUpdate != null && request.UsersToUpdate.Any())
        {
            var usersToUpdateDto = request.UsersToUpdate.ToDictionary(user => user.Id);
            var usersToUpdate = await _db.Users
                .Include(user => user.Roles)
                .Where(user => usersToUpdateDto.Keys.Contains(user.Id))
                .ToListAsync();

            var rolesInTheDepartment = await _db.Roles
                .Where(role => role.DepartmentId.HasValue && role.DepartmentId.Value == department.Id)
                .ToListAsync();

            foreach (var user in usersToUpdate)
            {
                var newRoles = rolesInTheDepartment
                    .Where(role => usersToUpdateDto[user.Id].RolesInDepartment?.Contains(role.RoleType.ToRoleName()) == true)
                    .ToList();
                var existedRoles = user.Roles!
                    .Where(role => role.DepartmentId.HasValue && role.DepartmentId.Value == department.Id)
                    .ToList();
                existedRoles
                    .ExceptBy(newRoles.Select(role => role.Id), role => role.Id)
                    .Select(role => user.Roles!.Single(userRole => role.Id == userRole.Id))
                    .ToList()
                    .ForEach(role => user.Roles!.Remove(role));
                newRoles
                    .ExceptBy(existedRoles.Select(role => role.Id), role => role.Id)
                    .ToList()
                    .ForEach(role => user.Roles!.Add(role)
                    );
            }

            await _db.SaveChangesAsync();
        }
        var users = await _db.Users
            .AsNoTracking()
            .Include(user => user.Roles)
            .ToListAsync();

        return new PerformUserAdminPanelResponse()
        {
            Users = users
                .Select(user =>
                {
                    var rolesInDepartment = user.Roles?
                        .Where(role => role.DepartmentId.HasValue && role.DepartmentId.Value == department.Id)
                        .Select(role => role.RoleType.ToRoleName())
                        .ToList();
                    return new UserDto()
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        RolesInDepartment = rolesInDepartment
                    };
                })
                .ToList()
        };
    }

    private IEnumerable<string> FormRoles(IEnumerable<Role> roles, bool includeSimpleRoles)
    {
        foreach (var role in roles)
        {
            var roleName = Enum.GetName(typeof(RoleType), role.RoleType)!;
            if (role.MatchedDepartment == null)
            {
                yield return roleName;
            }
            else
            {
                if (includeSimpleRoles)
                {
                    yield return roleName;
                }

                yield return $"{roleName}:{role.MatchedDepartment.UrlTitle}";
            }
        }
    }
}