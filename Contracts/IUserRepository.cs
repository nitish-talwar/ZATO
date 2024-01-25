using ZATO.DTOs;
using ZATO.Models;

namespace ZATO.Contracts;

public interface IUserRepository
{
    Task<User> GetAsync(Guid userId);

    Task<List<User>> GetAllAsync();

    Task<User> CreateAsync(User user);

    Task<int> DeleteAsync(Guid id);

    Task UpdateAsync(User category);
}