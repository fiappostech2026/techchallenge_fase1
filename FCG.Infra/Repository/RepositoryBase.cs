using FCG.Domain.Interfaces.IRepository;
using FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infra.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly FcgContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(FcgContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> ObterPorIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<List<T>> ObterTodosAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task AdicionarAsync(T entidade)
        {
            await _dbSet.AddAsync(entidade);
        }

        public void Atualizar(T entidade)
        {
            _dbSet.Update(entidade);
        }

        public void Remover(T entidade)
        {
            _dbSet.Remove(entidade);
        }

        public async Task SalvarAlteracoesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}