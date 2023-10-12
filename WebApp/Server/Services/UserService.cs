using Microsoft.EntityFrameworkCore;
using Models;

namespace WebApp.Server.Services;

public interface IUserService
{
    Task<Guid> GetUserOnLoginByVk(long vkId, string firstName, string lastName);
}

public class UserService: IUserService
{
    private readonly ApplicationDbContext _db;

    public UserService(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> GetUserOnLoginByVk(long vkId, string firstName, string lastName)
    {
        var user = await _db.Users.AsNoTracking().SingleOrDefaultAsync(user => user.VkId == vkId);
        if (user == null)
        {
            user = new User()
            {
                VkId = vkId,
                FirstName = firstName,
                LastName = lastName
            };
            await _db.Users.AddAsync(user);
            await _db.SaveChangesAsync();
        }
        else
        {
            user.FirstName = firstName;
            user.LastName = lastName;
            _db.Update(user);
        }

        return user.Id;
    }
}