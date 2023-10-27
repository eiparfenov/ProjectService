using Models;

namespace WebApp.Shared;

public static class NamingsExtensions
{
    public static string ToRoleName(this RoleType roleType)
    {
        return roleType switch
        {
            RoleType.Undefined => nameof(RoleType.Undefined),
            RoleType.Student => nameof(RoleType.Student),
            RoleType.Employee => nameof(RoleType.Employee),
            RoleType.Receptionist => nameof(RoleType.Receptionist),
            RoleType.AdminDepartment => nameof(RoleType.AdminDepartment),
            RoleType.SuperAdmin => nameof(RoleType.SuperAdmin),
            _ => throw new ArgumentOutOfRangeException(nameof(roleType), roleType, null)
        };
    }
}