using FCG.Domain.Interfaces.IRepository;
using FCG.Domain.Interfaces.IService;
using FCG.Domain.Service;
using FCG.Domain.Validators;
using FCG.Infra.Repository;
using FluentValidation;

namespace FCG.Extensions;

public static class DependencyInjectionExtension
{
    public static IServiceCollection AddDependencies(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IJwtService, JwtService>();
        
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IJogoRepository, JogoRepository>();
        services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();

        // Services
        services.AddScoped<IBibliotecaService, BibliotecaService>();

        // Validators
        services.AddValidatorsFromAssemblyContaining<UserValidator>();

        return services;
    }
}