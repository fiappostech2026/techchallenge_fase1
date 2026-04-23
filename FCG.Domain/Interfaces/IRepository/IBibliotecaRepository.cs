using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Domain.Interfaces.IRepository
{
    public interface IBibliotecaRepository : IRepositoryBase<Biblioteca>
    {
        Task<bool> UsuarioJaPossuiJogo(Guid userId, Guid jogoId);       
    }
}
