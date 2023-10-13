using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace WebApp.Shared.Grpc;

[Service("rtu-tc.reservation.user-service")]
public interface IUserGrpcService
{
    Task<GetAuthenticatedUserResponse> GetAuthenticatedUser(GetAuthenticatedUserRequest request, CallContext callContext = default!);
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