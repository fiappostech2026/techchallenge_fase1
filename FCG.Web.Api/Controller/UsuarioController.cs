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
    private readonly ILogger<UsuarioController> _logger;

    public UsuarioController(IUsuarioRepository usuarioRepository, IValidator<UsuarioDto> validator, ILogger<UsuarioController> logger)
    {
        _usuarioRepository = usuarioRepository;
        _validator = validator;
        _logger = logger;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Usuario>>> ObterTodos()
    {
        try
        {
            var usuarios = await _usuarioRepository.ObterTodosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar lista de usuários.");
            throw;
        }
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<Usuario>> ObterPorId(Guid id)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario is null)
                return NotFound();

            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar usuário {UsuarioId}.", id);
            throw;
        }
    }

    [HttpPost("criar")]
    [Authorize]
    public async Task<ActionResult<Usuario>> Criar([FromBody] UsuarioDto dto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar usuário com e-mail {Email}.", dto.Email);
            throw;
        }
    }

    [HttpPut("atualizar/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Usuario>> Atualizar(Guid id, [FromBody] UsuarioDto dto)
    {
        try
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar usuário {UsuarioId}.", id);
            throw;
        }
    }

    [HttpDelete("excluir/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Excluir(Guid id)
    {
        try
        {
            var excluido = await _usuarioRepository.ExcluirAsync(id);

            if (!excluido)
                return NotFound();

            return Ok(new { Mensagem = "Usuário deletado com sucesso!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir usuário {UsuarioId}.", id);
            throw;
        }
    }

    [HttpPatch("{id:guid}/promover")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Promover(Guid id)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorIdAsync(id);

            if (usuario is null)
                return NotFound();

            if (usuario.Perfil == PerfilEnum.Admin)
                return BadRequest(new { Mensagem = "Usuário já é Admin." });

            await _usuarioRepository.PromoverParaAdminAsync(id);

            return Ok(new { Mensagem = "Usuário promovido para Admin com sucesso!" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao promover usuário {UsuarioId}.", id);
            throw;
        }
    }
}
