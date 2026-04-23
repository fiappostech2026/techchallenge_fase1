using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Infra.Repository
{
    public class BibliotecaRepository : RepositoryBase<Biblioteca>, IBibliotecaRepository
    {
        public BibliotecaRepository(FcgContext context) : base(context)
        {
        }

        public async Task<bool> UsuarioJaPossuiJogo(Guid userId, Guid jogoId)
        {
            return await _dbSet.AnyAsync(x => x.UserId == userId && x.JogoId == jogoId);
        }
    }
}
