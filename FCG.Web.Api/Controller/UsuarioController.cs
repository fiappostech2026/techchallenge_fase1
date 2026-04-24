using FCG.Domain.Dto;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Interfaces.IRepository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Controller;

[ApiController]
[Route("api/v1/usuarios")]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IValidator<UsuarioDto> _validator;

    public UsuarioController(IUsuarioRepository usuarioRepository, IValidator<UsuarioDto> validator)
    {
        _usuarioRepository = usuarioRepository;
        _validator = validator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Usuario>>> ObterTodos()
    {
        var usuarios = await _usuarioRepository.ObterTodosAsync();
        return Ok(usuarios);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<Usuario>> ObterPorId(Guid id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);

        if (usuario is null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPost("criar")]
    [Authorize]
    public async Task<ActionResult<Usuario>> Criar([FromBody] UsuarioDto dto)
    {
        var validacao = await _validator.ValidateAsync(dto);

        if (!validacao.IsValid)
            return BadRequest(validacao.Errors.Select(e => e.ErrorMessage));

        var usuarioCriado = await _usuarioRepository.CriarAsync(dto);

        return Created($"api/usuarios/{usuarioCriado.Id}", new
        {
            Mensagem = "Usuário criado com sucesso!",
            Usuario = usuarioCriado
        });
    }

    [HttpPut("atualizar/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Usuario>> Atualizar(Guid id, [FromBody] UsuarioDto dto)
    {
        var validacao = await _validator.ValidateAsync(dto);

        if (!validacao.IsValid)
            return BadRequest(validacao.Errors.Select(e => e.ErrorMessage));

        var usuario = await _usuarioRepository.AtualizarAsync(id, dto);

        if (usuario is null)
            return NotFound();

        return Ok(new
        {
            Mensagem = "Usuário atualizado com sucesso!",
            Usuario = usuario
        });
    }

    [HttpDelete("excluir/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        var excluido = await _usuarioRepository.ExcluirAsync(id);

        if (!excluido)
            return NotFound();

        return Ok(new { Mensagem = "Usuário deletado com sucesso!" });
    }

    [HttpPatch("{id:guid}/promover")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Promover(Guid id)
    {
        var usuario = await _usuarioRepository.ObterPorIdAsync(id);

        if (usuario is null)
            return NotFound();

        if (usuario.Perfil == PerfilEnum.Admin)
            return BadRequest(new { Mensagem = "Usuário já é Admin." });

        await _usuarioRepository.PromoverParaAdminAsync(id);

        return Ok(new { Mensagem = "Usuário promovido para Admin com sucesso!" });
    }
}