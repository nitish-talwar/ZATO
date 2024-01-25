using Microsoft.EntityFrameworkCore;
using ZATO.Contracts;
using ZATO.Models;

namespace ZATO.Repository;

public class UserRepository : IUserRepository
{
    private readonly ZATOStoreContext _context;
    
    public UserRepository(ZATOStoreContext context) => this._context = context;

    public async Task<User> GetAsync(Guid userId)
    {
        if(userId == null)
        {
            return null;
        }

        var user = await this._context.Users.FirstOrDefaultAsync((x) => x.Id == userId);
        return user; 
    }

    public async Task<List<User>> GetAllAsync()
    {
        var users = await this._context.Users.ToListAsync();
        return users;
    }

    public async Task<User> CreateAsync(User user)
    {
            await this._context.AddAsync(user);
            this._context.SaveChanges();
            return user;
    }

    public async Task<int> DeleteAsync(Guid userId)
    {
        this._context.Remove(new User() {Id = userId});
        return await this._context.SaveChangesAsync();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }
}