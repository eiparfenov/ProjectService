using Microsoft.EntityFrameworkCore;
using Models;
using WebApp.Shared.Grpc;

namespace WebApp.Server.Grpc;

public class WorkplaceGrpcService: IWorkplaceGrpcService
{
    private readonly ApplicationDbContext _db;

    public WorkplaceGrpcService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<GetAllWorkplacesResponse> GetAllWorkplaces(GetAllWorkplacesRequest request)
    {
        var department = await _db.Departments.SingleAsync(dep => dep.UrlTitle == request.DepartmentUrl);

        var workplaces = await _db.Workplaces
            .AsNoTracking()
            .Where(wp => wp.DepartmentId == department.Id)
            .OrderBy(wp => wp.Title)
            .Include(wp => wp.EquipmentModels)
            .Select(wp => new WorkplaceDto()
            {
                Id = wp.Id,
                Title = wp.Title,
                Comment = wp.Comment,
                State = AdminPanelState.Initial,
                EquipmentModels = wp.EquipmentModels!
                    .Select(eqModel => eqModel.Id)
                    .ToList()
            })
            .ToListAsync();
        return new GetAllWorkplacesResponse()
        {
            Workplaces = workplaces
        };
    }

    public async Task<GetAllEquipmentModelsResponse> GetAllEquipmentModels(GetAllEquipmentModelsRequest request)
    {
        var department = await _db.Departments.SingleAsync(dep => dep.UrlTitle == request.DepartmentUrl);

        var eqModels = await _db.EquipmentModels
            .AsNoTracking()
            .Where(eqModel => eqModel.DepartmentId == department.Id)
            .Select(eqModel => ToEquipmentModelOnWorkplaceDto(eqModel))
            .ToListAsync();

        return new GetAllEquipmentModelsResponse()
        {
            EquipmentModels = eqModels
        };
    }

    public async Task<PerformWorkplaceAdminPanelResponse> PerformWorkplaceAdminPanel(PerformWorkplaceAdminPanelRequest request)
    {
        if (request.EditedWorkplaces == null || !request.EditedWorkplaces.Any()) return new PerformWorkplaceAdminPanelResponse();
        
        var department = await _db.Departments.SingleAsync(dep => dep.UrlTitle == request.DepartmentUrl);

        var workplacesToUpdateDto = request.EditedWorkplaces
            .Where(dto => dto.State == AdminPanelState.Update)
            .ToDictionary(dto => dto.Id);

        var eqModelsInDepartment = await _db.EquipmentModels
            .Where(eqModel => eqModel.DepartmentId == department.Id)
            .ToDictionaryAsync(x => x.Id);

        var workplacesToEdit = await _db.Workplaces
            .Where(wp => wp.DepartmentId == department.Id)
            .Where(wp => workplacesToUpdateDto.Keys.Contains(wp.Id))
            .Include(wp => wp.EquipmentModels)
            .ToListAsync();
        
        workplacesToEdit.ForEach(wp =>
        {
            var dto = workplacesToUpdateDto[wp.Id];
            wp.Title = dto.Title;
            wp.Comment = dto.Comment;
            var eqModelsToRemoveIds = 
            wp.EquipmentModels = dto.EquipmentModels?
                .Select(eqModelDto => eqModelsInDepartment[eqModelDto])
                .ToList();
        });

        var workplacesToDeleteIds = request.EditedWorkplaces
            .Where(wp => wp.State == AdminPanelState.Delete)
            .Select(wp => wp.Id)
            .ToHashSet();

        await _db.Workplaces
            .Where(wp => wp.DepartmentId == department.Id)
            .Where(wp => workplacesToDeleteIds.Contains(wp.Id))
            .ExecuteDeleteAsync();

        var workplacesToCreate = request.EditedWorkplaces
            .Where(wp => wp.State == AdminPanelState.Create)
            .Select(dto => new Workplace()
            {
                DepartmentId = department.Id,
                Title = dto.Title,
                Comment = dto.Comment,
                EquipmentModels = dto.EquipmentModels?
                    .Select(eqModelDto => eqModelsInDepartment[eqModelDto])
                    .ToList()
            });

        await _db.Workplaces.AddRangeAsync(workplacesToCreate);

        await _db.SaveChangesAsync();
        return new PerformWorkplaceAdminPanelResponse();
    }

    private static EquipmentModelOnWorkplaceDto ToEquipmentModelOnWorkplaceDto(EquipmentModel model)
    {
        return new EquipmentModelOnWorkplaceDto()
        {
            Id = model.Id,
            Title = model.Title
        };
    }
}