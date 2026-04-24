using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Infra.Context;

namespace FCG.Extensions;

public static class MasterUserExtension
{
    public static void AddMasterUser(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FcgContext>();

        if (!db.Usuarios.Any(u => u.Perfil == PerfilEnum.Admin))
        {
            db.Usuarios.Add(new Usuario
            {
                Nome = "Master",
                Email = "admin@fcg.com",
                Senha = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Perfil = PerfilEnum.Admin
            });
            db.SaveChanges();
        }
    }
}