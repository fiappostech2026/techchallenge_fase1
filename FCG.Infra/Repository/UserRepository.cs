using FCG.Domain.Dto;
using FCG.Domain.Entitie;
using FCG.Domain.Enum;
using FCG.Domain.Interfaces.IRepository;
using FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly FcgContext _context;

    public UserRepository(FcgContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users
            .ToListAsync();
    }

    public async Task<User> PostAsync(UserDto user)
    {
        User data = new User
        {
            Id = Guid.NewGuid(),
            Name = user.Name,
            Email = user.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(user.Password)
        };
        await _context.Users.AddAsync(data);
        
        await _context.SaveChangesAsync();
        return data;
    }

    public async Task<User?> PutAsync(Guid id, UserDto updatedUser)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return null;

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Password = BCrypt.Net.BCrypt.HashPassword(updatedUser.Password);

        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }
    
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    
    public async Task PromoteToAdminAsync(Guid id)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        if (user is null) return;

        user.Role = RoleEnum.Admin;
        await _context.SaveChangesAsync();
    }
}