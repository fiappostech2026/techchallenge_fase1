using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Infra.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Infra.Repository
{
    public class JogoRepository : RepositoryBase<Jogo>, IJogoRepository
    {
        public JogoRepository(FcgContext context) : base(context)
        {
        }
    }
}
