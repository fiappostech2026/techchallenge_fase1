using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces.IService;

public interface IJwtService
{
    string GerarToken(Usuario usuario);
}