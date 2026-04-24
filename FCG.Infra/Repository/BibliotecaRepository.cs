using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infra.Repository
{
    public class BibliotecaRepository : RepositoryBase<Biblioteca>, IBibliotecaRepository
    {
        public BibliotecaRepository(FcgContext context) : base(context)
        {
        }

        public async Task<bool> UsuarioJaPossuiJogo(Guid usuarioId, Guid jogoId)
        {
            return await _dbSet.AnyAsync(x => x.UsuarioId == usuarioId && x.JogoId == jogoId);
        }
    }
}