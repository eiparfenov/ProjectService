using ProtoBuf;

namespace WebApp.Shared.Grpc;

public interface IDepartmentGrpcService
{
    
}

[ProtoContract]
public class DepartmentDto
{
    [ProtoMember(1)]public required Guid Id { get; set; }
    [ProtoMember(2)]public required string Title { get; set; }
    [ProtoMember(3)]public required string UrlTitle { get; set; }
}

[ProtoContract]
public class ReadAllDepartmentsRequest
{
    
}

[ProtoContract]
public class ReadAllDepartmentsResponse
{
    public List<DepartmentDto>? Departments { get; set; }
}

[ProtoContract]
public class UpdateDepartmentsRequest
{
    public List<DepartmentDto>? DepartmentsToUpdate { get; set; }
    public List<DepartmentDto>? DepartmentsToAdd { get; set; }
    public List<Guid>? DepartmentsToDelete { get; set; }
}

[ProtoContract]
public class UpdateAllDepartmentsResponse
{
    public List<DepartmentDto>? Departments { get; set; }
}