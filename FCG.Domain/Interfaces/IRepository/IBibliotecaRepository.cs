using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces.IRepository
{
    public interface IBibliotecaRepository : IRepositoryBase<Biblioteca>
    {
        Task<bool> UsuarioJaPossuiJogo(Guid usuarioId, Guid jogoId);
    }
}