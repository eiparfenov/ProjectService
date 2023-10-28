using Microsoft.EntityFrameworkCore;
using Models;
using WebApp.Shared.Grpc;

namespace WebApp.Server.Grpc;

public class EquipmentGrpcService : IEquipmentGrpcService
{
    private readonly ApplicationDbContext _db;

    public EquipmentGrpcService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<PerformEquipmentAdminPanelResponse> PerformEquipmentAdminPanel(PerformEquipmentAdminPanelRequest request)
    {
        if (request.EquipmentModelsWithChanges == null || !request.EquipmentModelsWithChanges.Any()) return new PerformEquipmentAdminPanelResponse();

        var eqModelsToUpdateDto = request.EquipmentModelsWithChanges
            .Where(dto => dto.State is AdminPanelState.Update or AdminPanelState.Initial)
            .ToDictionary(dto => dto.Id);
        
        var department = await _db.Departments.SingleAsync(dep => dep.UrlTitle == request.DepartmentUrl);
        
        var eqModelsToUpdate = await _db.EquipmentModels
            .Where(eqModel => eqModel.DepartmentId == department.Id)
            .Where(eqModel => eqModelsToUpdateDto.Keys.Contains(eqModel.Id))
            .Include(eqModel => eqModel.Equipments)
            .ToListAsync();

        foreach (var eqModel in eqModelsToUpdate)
        {
            var dto = eqModelsToUpdateDto[eqModel.Id];
            
            eqModel.Title = dto.Title;
            eqModel.Description = dto.Description;
            
            if(dto.Equipments == null || !dto.Equipments.Any()) continue;

            var equipmentsToUpdateDto = dto.Equipments
                .Where(eq => eq.State == AdminPanelState.Update)
                .ToDictionary(x => x.Id);
            
            eqModel.Equipments!.ForEach(eq =>
            {
                if (!equipmentsToUpdateDto.TryGetValue(eq.Id, out var eqDto)) return;
                
                eq.InvNumber = eqDto.InvNumber;
                eq.Comment = eqDto.Comment;
            });
            
            var equipmentsToDeleteIds = dto.Equipments
                .Where(eq => eq.State == AdminPanelState.Delete)
                .Select(eq => eq.Id)
                .ToHashSet();

            eqModel.Equipments!.RemoveAll(eq => equipmentsToDeleteIds.Contains(eq.Id));

            var equipmentsToCreateDto = dto.Equipments
                .Where(eq => eq.State == AdminPanelState.Create)
                .ToList();
            
            equipmentsToCreateDto.ForEach(eqDto =>
            {
                eqModel.Equipments.Add(new Equipment()
                {
                    InvNumber = eqDto.InvNumber,
                    Comment = eqDto.Comment
                });
            });
        }

        var eqModelsToDeleteIds = request.EquipmentModelsWithChanges
            .Where(eqModel => eqModel.State == AdminPanelState.Delete)
            .Select(eqModel => eqModel.Id)
            .ToHashSet();

        await _db.EquipmentModels
            .Where(eqModel => eqModel.DepartmentId == department.Id)
            .Where(eqModel => eqModelsToDeleteIds.Contains(eqModel.Id))
            .ExecuteDeleteAsync();

        var equipmentModelsToCreate = request.EquipmentModelsWithChanges
            .Where(eqModel => eqModel.State == AdminPanelState.Create)
            .ToList()
            .Select(eqModelDto => new EquipmentModel()
            {
                DepartmentId = department.Id,
                Title = eqModelDto.Title,
                Description = eqModelDto.Description,
                Equipments = eqModelDto.Equipments?
                    .Where(eqDto => eqDto.State == AdminPanelState.Create)
                    .Select(eqDto => new Equipment()
                    {
                        InvNumber = eqDto.InvNumber,
                        Comment = eqDto.Comment
                    })
                    .ToList()
            })
            .ToList();

        await _db.EquipmentModels.AddRangeAsync(equipmentModelsToCreate);

        await _db.SaveChangesAsync();

        return new PerformEquipmentAdminPanelResponse();
    }

    public async Task<GetEquipmentModelsResponse> GetEquipmentModels(GetEquipmentModelsRequest request)
    {
        var department = await _db.Departments.SingleAsync(dep => dep.UrlTitle == request.DepartmentUrl);

        var equipmentModels = await _db.EquipmentModels
            .Where(model => model.DepartmentId == department.Id)
            .OrderBy(model => model.Title)
            .Select(eqModel => new EquipmentModelDto()
            {
                Id = eqModel.Id,
                Title = eqModel.Title,
                Description = eqModel.Description,
                State = AdminPanelState.Initial,
            })
            .ToListAsync();

        return new GetEquipmentModelsResponse()
        {
            EquipmentModels = equipmentModels
        };
    }

    public async Task<GetEquipmentResponse> GetEquipment(GetEquipmentRequest request)
    {
        var equipments = await _db.Equipments
            .Where(equipment => equipment.EquipmentModelId == request.EquipmentModelId)
            .OrderBy(equipment => equipment.InvNumber)
            .Select(equipment => new EquipmentDto()
            {
                Id = equipment.Id,
                InvNumber = equipment.InvNumber,
                Comment = equipment.Comment,
                State = AdminPanelState.Initial,
            })
            .ToListAsync();
        return new GetEquipmentResponse()
        {
            Equipments = equipments
        };
    }
}