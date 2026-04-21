using FCG.Domain.Entitie;

namespace FCG.Domain.Interfaces.IService;

public interface IJwtService
{
    string GenerateToken(User user);
}