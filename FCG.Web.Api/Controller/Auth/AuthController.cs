using FCG.Domain.Dto;
using FCG.Domain.Interfaces.IRepository;
using FCG.Domain.Interfaces.IService;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Controller.Auth;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IJwtService _jwtService;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IUsuarioRepository usuarioRepository, IJwtService jwtService, ILogger<AuthController> logger)
    {
        _usuarioRepository = usuarioRepository;
        _jwtService = jwtService;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(dto.Email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha))
                return Unauthorized(new { Mensagem = "E-mail ou senha inválidos." });

            var token = _jwtService.GerarToken(usuario);

            return Ok(new
            {
                Mensagem = "Login realizado com sucesso!",
                Token = token
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao realizar login para o e-mail {Email}.", dto.Email);
            throw;
        }
    }
}
