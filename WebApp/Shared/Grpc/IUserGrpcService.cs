using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace WebApp.Shared.Grpc;

[Service("rtu-tc.reservation.user-service")]
public interface IUserGrpcService
{
    Task<GetAuthenticatedUserResponse> GetAuthenticatedUser(GetAuthenticatedUserRequest request);
    Task<GetAllRolesInDepartmentResponse> GetAllRolesInDepartment(GetAllRolesInDepartmentRequest request);
    Task<PerformUserAdminPanelResponse> PerformUserAdminPanel(PerformUserAdminPanelRequest request);
}

[ProtoContract]
public class GetAuthenticatedUserRequest
{
    
}

[ProtoContract]
[ProtoInclude(2, typeof(GetAuthenticatedUserResponseAuthorized))]
[ProtoInclude(3, typeof(GetAuthenticatedUserResponseNotAuthorized))]
public class GetAuthenticatedUserResponse
{
    
}
[ProtoContract]
public class GetAuthenticatedUserResponseAuthorized: GetAuthenticatedUserResponse
{
    [ProtoMember(1)]public string? FirstName { get; set; }
    [ProtoMember(2)]public string? LastName { get; set; }
    [ProtoMember(3)]public List<string>? Roles { get; set; }
}
[ProtoContract]
public class GetAuthenticatedUserResponseNotAuthorized : GetAuthenticatedUserResponse
{
    
}

[ProtoContract]
public class GetAllRolesInDepartmentRequest
{
}

[ProtoContract]
public class GetAllRolesInDepartmentResponse
{
    [ProtoMember(1)] public List<string>? Roles { get; set; }
}

[ProtoContract]
public class UserDto
{
    [ProtoMember(1)] public Guid Id { get; set; }
    [ProtoMember(2)] public string? FirstName { get; set; }
    [ProtoMember(3)] public string? MiddleName { get; set; }
    [ProtoMember(4)] public string? LastName { get; set; }
    [ProtoMember(5)] public List<string>? RolesInDepartment { get; set; }
    public bool Changed { get; set; }
}

[ProtoContract]
public class PerformUserAdminPanelRequest
{
    [ProtoMember(1)] public string? DepartmentUrl { get; set; }
    [ProtoMember(2)] public List<UserDto>? UsersToUpdate { get; set; }
}

[ProtoContract]
public class PerformUserAdminPanelResponse
{
    [ProtoMember(1)] public List<UserDto>? Users { get; set; }
}
