using FCG.Domain.Dto;
using FCG.Domain.Entities;

namespace FCG.Domain.Interfaces.IRepository;

public interface IUsuarioRepository
{
    Task<Usuario?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Usuario>> ObterTodosAsync();
    Task<Usuario> CriarAsync(UsuarioDto usuario);
    Task<Usuario?> AtualizarAsync(Guid id, UsuarioDto usuarioAtualizado);
    Task<bool> ExcluirAsync(Guid id);
    Task<Usuario?> ObterPorEmailAsync(string email);
    Task PromoverParaAdminAsync(Guid id);
}