using FCG.Domain.Dto;
using FCG.Domain.Entitie;

namespace FCG.Domain.Interfaces.IRepository;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<IEnumerable<User>> GetAllAsync();
    Task<User> PostAsync(UserDto user);
    Task<User?> PutAsync(Guid id, UserDto updatedUser);
    Task<bool> DeleteAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task PromoteToAdminAsync(Guid id);
}