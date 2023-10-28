using ProtoBuf;
using ProtoBuf.Grpc.Configuration;

namespace WebApp.Shared.Grpc;

[Service("rtu-tc.reservation.workplace-service")]
public interface IWorkplaceGrpcService
{
    Task<GetAllWorkplacesResponse> GetAllWorkplaces(GetAllWorkplacesRequest request);
    Task<GetAllEquipmentModelsResponse> GetAllEquipmentModels(GetAllEquipmentModelsRequest request);
    Task<PerformWorkplaceAdminPanelResponse> PerformWorkplaceAdminPanel(PerformWorkplaceAdminPanelRequest request);
}

[ProtoContract]
public class EquipmentModelOnWorkplaceDto
{
    [ProtoMember(1)] public Guid Id { get; set; }
    [ProtoMember(2)] public string? Title { get; set; }

    public override string ToString()
    {
        return Title!;
    }
}

[ProtoContract]
public class WorkplaceDto: IHasAdminPanelState
{
    [ProtoMember(1)] public Guid Id { get; set; }
    [ProtoMember(2)] public string? Title { get; set; }
    [ProtoMember(3)] public string? Comment { get; set; }
    [ProtoMember(4)] public List<Guid>? EquipmentModels { get; set; }
    [ProtoMember(5)] public AdminPanelState State { get; set; }

    public IEnumerable<Guid> EquipmentModelToBind
    {
        get => EquipmentModels?.AsEnumerable() ?? Enumerable.Empty<Guid>();
        set
        {
            var newValues = value.ToList();
            if(newValues.Count == EquipmentModels?.Count) return;
            EquipmentModels = newValues;
            this.PerformUpdate();
        }
    }
    public AdminPanelState PreviousState { get; set; }
}

[ProtoContract]
public class GetAllWorkplacesRequest
{
    [ProtoMember(1)] public string? DepartmentUrl { get; set; }
}

[ProtoContract]
public class GetAllWorkplacesResponse
{
    [ProtoMember(1)] public List<WorkplaceDto>? Workplaces { get; set; }
}

[ProtoContract]
public class GetAllEquipmentModelsRequest
{
    [ProtoMember(1)] public string? DepartmentUrl { get; set; }
}

[ProtoContract]
public class GetAllEquipmentModelsResponse
{
    [ProtoMember(1)] public List<EquipmentModelOnWorkplaceDto>? EquipmentModels { get; set; }
}

[ProtoContract]
public class PerformWorkplaceAdminPanelRequest
{
    [ProtoMember(1)] public string? DepartmentUrl { get; set; }
    [ProtoMember(2)] public List<WorkplaceDto>? EditedWorkplaces { get; set; }
}

[ProtoContract]
public class PerformWorkplaceAdminPanelResponse
{
    
}