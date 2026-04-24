namespace FCG.Domain.Interfaces.IService
{
    public interface IBibliotecaService
    {
        Task<bool> ComprarJogo(Guid usuarioId, Guid jogoId);
    }
}