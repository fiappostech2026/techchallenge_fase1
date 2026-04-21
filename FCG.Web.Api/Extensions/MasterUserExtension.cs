using FCG.Domain.Entitie;
using FCG.Domain.Enum;
using FCG.Infra.Context;

namespace FCG.Extensions;

public static class MasterUserExtension
{
    public static void AddMasterUser(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FcgContext>();

        if (!db.Users.Any(u => u.Role == RoleEnum.Admin))
        {
            db.Users.Add(new User
            {
                Name = "Master",
                Email = "admin@fcg.com",
                Password = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = RoleEnum.Admin
            });
            db.SaveChanges();
        }
    }
}