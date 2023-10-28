using ProtoBuf;
using ProtoBuf.Grpc.Configuration;

namespace WebApp.Shared.Grpc;

[Service("rtu-tc.reservation.equipment-service")]
public interface IEquipmentGrpcService
{
    Task<PerformAdminPanelResponse> PerformAdminPanel(PerformAdminPanelRequest request);
    Task<GetEquipmentModelsResponse> GetEquipmentModels(GetEquipmentModelsRequest request);
    Task<GetEquipmentResponse> GetEquipment(GetEquipmentRequest request);
}

[ProtoContract]
public class EquipmentModelDto: IHasAdminPanelState
{
    [ProtoMember(1)] public Guid Id { get; set; }
    [ProtoMember(2)] public string? Title { get; set; }
    [ProtoMember(3)] public string? Description { get; set; }
    [ProtoMember(4)] public AdminPanelState State { get; set; }
    [ProtoMember(5)] public List<EquipmentDto>? Equipments { get; set; }
    public AdminPanelState PreviousState { get; set; }
}

[ProtoContract]
public class EquipmentDto: IHasAdminPanelState
{
    [ProtoMember(1)] public Guid Id { get; set; }
    [ProtoMember(2)] public string? InvNumber { get; set; }
    [ProtoMember(3)] public string? Comment { get; set; }
    [ProtoMember(4)] public AdminPanelState State { get; set; }
    public AdminPanelState PreviousState { get; set; }
}

[ProtoContract]
public class PerformAdminPanelRequest
{
    [ProtoMember(1)] public string? DepartmentUrl { get; set; }
    [ProtoMember(2)] public List<EquipmentModelDto>? EquipmentModelsWithChanges { get; set; }
}

[ProtoContract]
public class PerformAdminPanelResponse
{
    
}

[ProtoContract]
public class GetEquipmentModelsRequest
{
    [ProtoMember(1)] public string? DepartmentUrl { get; set; }
}

[ProtoContract]
public class GetEquipmentModelsResponse
{
    [ProtoMember(1)] public List<EquipmentModelDto>? EquipmentModels { get; set; }
}

[ProtoContract]
public class GetEquipmentRequest
{
    [ProtoMember(1)] public Guid EquipmentModelId { get; set; }        
}

[ProtoContract]
public class GetEquipmentResponse
{
    [ProtoMember(2)] public List<EquipmentDto>? Equipments { get; set; }
}