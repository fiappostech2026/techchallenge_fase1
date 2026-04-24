using FCG.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Domain.Interfaces.IService
{
    public interface IBibliotecaService
    {
        Task<bool> ComprarJogo(Guid userId, Guid jogoId);
    }
}
