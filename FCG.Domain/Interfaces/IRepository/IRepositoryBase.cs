namespace FCG.Domain.Interfaces.IRepository
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<T> ObterPorIdAsync(Guid id);
        Task<List<T>> ObterTodosAsync();
        Task AdicionarAsync(T entidade);
        void Atualizar(T entidade);
        void Remover(T entidade);
        Task SalvarAlteracoesAsync();
    }
}