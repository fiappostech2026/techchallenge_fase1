using FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Extensions;

public static class DatabaseExtension
{
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<FcgContext>(opt =>
            opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

        return builder;
    }
    
    public static WebApplication MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<FcgContext>();
        db.Database.Migrate(); 
        
        return app;
    }
}