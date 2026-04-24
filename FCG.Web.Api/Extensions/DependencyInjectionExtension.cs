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
        // Serviços
        services.AddScoped<IJwtService, JwtService>();

        // Repositórios
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IJogoRepository, JogoRepository>();
        services.AddScoped<IBibliotecaRepository, BibliotecaRepository>();

        // Serviços
        services.AddScoped<IBibliotecaService, BibliotecaService>();

        // Validadores
        services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();

        return services;
    }
}