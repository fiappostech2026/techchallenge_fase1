using FCG.Domain.Dto;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Interfaces.IRepository;
using FCG.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infra.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly FcgContext _context;

    public UsuarioRepository(FcgContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObterPorIdAsync(Guid id)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<Usuario>> ObterTodosAsync()
    {
        return await _context.Usuarios
            .ToListAsync();
    }

    public async Task<Usuario> CriarAsync(UsuarioDto usuario)
    {
        var dados = new Usuario
        {
            Id = Guid.NewGuid(),
            Nome = usuario.Nome,
            Email = usuario.Email,
            Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha)
        };
        await _context.Usuarios.AddAsync(dados);

        await _context.SaveChangesAsync();
        return dados;
    }

    public async Task<Usuario?> AtualizarAsync(Guid id, UsuarioDto usuarioAtualizado)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario is null)
            return null;

        usuario.Nome = usuarioAtualizado.Nome;
        usuario.Email = usuarioAtualizado.Email;
        usuario.Senha = BCrypt.Net.BCrypt.HashPassword(usuarioAtualizado.Senha);

        await _context.SaveChangesAsync();
        return usuario;
    }

    public async Task<bool> ExcluirAsync(Guid id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario is null)
            return false;

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email)
    {
        return await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task PromoverParaAdminAsync(Guid id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);

        if (usuario is null) return;

        usuario.Perfil = PerfilEnum.Admin;
        await _context.SaveChangesAsync();
    }
}