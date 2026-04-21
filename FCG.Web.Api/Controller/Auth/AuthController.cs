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
    private readonly IUserRepository _userRepository;

    public AuthController(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var user = await _userRepository.GetByEmailAsync(dto.Email);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            return Unauthorized(new { Message = "E-mail ou senha inválidos." });

        var token = _jwtService.GenerateToken(user);

        return Ok(new
        {
            Message = "Login realizado com sucesso!",
            Token = token
        });
    }
}