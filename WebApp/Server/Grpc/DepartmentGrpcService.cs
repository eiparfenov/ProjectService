using Microsoft.EntityFrameworkCore;
using Models;
using WebApp.Shared.Grpc;

namespace WebApp.Server.Grpc;

public class DepartmentGrpcService: IDepartmentGrpcService
{
    private readonly ApplicationDbContext db;

    public DepartmentGrpcService(ApplicationDbContext db)
    {
        this.db = db;
    }

    public async Task<ReadAllDepartmentsResponse> ReadAllDepartments(ReadAllDepartmentsRequest request)
    {
        var departments = await db.Departments
            .Select(dep => new DepartmentDto()
            {
                Id = dep.Id,
                UrlTitle = dep.UrlTitle,
                Title = dep.Title
            })
            .ToListAsync();

        return new ReadAllDepartmentsResponse()
        {
            Departments = departments
        };
    }

    public async Task<UpdateDepartmentsResponse> UpdateDepartments(UpdateDepartmentsRequest request)
    {
        if(request.DepartmentsToUpdate != null && request.DepartmentsToUpdate.Any())
        {
            var departmentsToUpdateDto = request.DepartmentsToUpdate
                .ToDictionary(dep => dep.Id);
            
            var departmentsToUpdate = await db.Departments
                .Where(dep => departmentsToUpdateDto.ContainsKey(dep.Id))
                .ToListAsync();
            
            departmentsToUpdate
                .ForEach(dep =>
                {
                    var dto = departmentsToUpdateDto[dep.Id];
                    dep.Title = dto.Title;
                    dep.UrlTitle = dto.UrlTitle;
                });
        }

        if (request.DepartmentsToAdd != null && request.DepartmentsToAdd.Any())
        {
            var departmentsToAdd = request.DepartmentsToAdd
                .Select(depDto => new Department()
                {
                    Title = depDto.Title,
                    UrlTitle = depDto.UrlTitle
                })
                .ToList();
            await db.AddRangeAsync(departmentsToAdd);
        }

        if (request.DepartmentsToDelete != null && request.DepartmentsToDelete.Any())
        {
            var departmentsToDeleteId = request.DepartmentsToDelete.ToHashSet();
            await db.Departments
                .Where(dep => departmentsToDeleteId.Contains(dep.Id))
                .ExecuteDeleteAsync();
        }

        await db.SaveChangesAsync();

        var departmentsAfterSave = await db.Departments
            .Select(dep => new DepartmentDto()
            {
                Id = dep.Id,
                Title = dep.Title,
                UrlTitle = dep.UrlTitle
            })
            .ToListAsync();
        return new UpdateDepartmentsResponse()
        {
            Departments = departmentsAfterSave
        };
    }
}