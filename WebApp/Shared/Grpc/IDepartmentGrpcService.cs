using ProtoBuf;
using ProtoBuf.Grpc.Configuration;

namespace WebApp.Shared.Grpc;

[Service("rtu-tc.reservation.department-service")]
public interface IDepartmentGrpcService
{
    Task<ReadAllDepartmentsResponse> ReadAllDepartments(ReadAllDepartmentsRequest request);
    Task<UpdateDepartmentsResponse> UpdateDepartments(UpdateDepartmentsRequest request);
}

[ProtoContract]
public class DepartmentDto
{
    [ProtoMember(1)] public Guid Id { get; set; }
    [ProtoMember(2)] public string Title { get; set; }
    [ProtoMember(3)] public string UrlTitle { get; set; }
    
    /// <summary>
    /// Use only on client
    /// </summary>
    public State DtoState { get; set; }
    public enum State
    {
        Initial,
        Updated,
        Created
    }
}

[ProtoContract]
public class ReadAllDepartmentsRequest
{
    
}

[ProtoContract]
public class ReadAllDepartmentsResponse
{
    [ProtoMember(1)] public List<DepartmentDto>? Departments { get; set; }
}

[ProtoContract]
public class UpdateDepartmentsRequest
{
    [ProtoMember(1)] public List<DepartmentDto>? DepartmentsToUpdate { get; set; }
    [ProtoMember(2)] public List<DepartmentDto>? DepartmentsToAdd { get; set; }
    [ProtoMember(3)] public List<Guid>? DepartmentsToDelete { get; set; }
}

[ProtoContract]
public class UpdateDepartmentsResponse
{
    [ProtoMember(1)] public List<DepartmentDto>? Departments { get; set; }
}