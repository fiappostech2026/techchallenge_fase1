using FCG.Domain.Dto;
using FCG.Domain.Entitie;
using FCG.Domain.Enum;
using FCG.Domain.Interfaces.IRepository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Controller;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IValidator<UserDto> _validator;

    public UserController(IUserRepository userRepository, IValidator<UserDto> validator)
    {
        _userRepository = userRepository;
        _validator = validator;
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<User>>> GetAll()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<ActionResult<User>> GetById(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        return Ok(user);
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<ActionResult<User>> Create([FromBody] UserDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var createdUser = await _userRepository.PostAsync(dto);
        
        return Created($"api/users/{createdUser.Id}", new
        {
            Message = "Usuário criado com sucesso!",
            User = createdUser
        });
    }

    [HttpPut("update/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<User>> Update(Guid id, [FromBody] UserDto dto)
    {
        var validation = await _validator.ValidateAsync(dto);

        if (!validation.IsValid)
            return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

        var user = await _userRepository.PutAsync(id, dto);

        if (user is null)
            return NotFound();

        return Ok(new
        {
            Message = "Usuário atualizado com sucesso!",
            User = user
        });
    }

    [HttpDelete("delete/{id:guid}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _userRepository.DeleteAsync(id);

        if (!deleted)
            return NotFound();


        return Ok(new { Message = "Usuário deletado com sucesso!" });
    }
    
    [HttpPatch("{id:guid}/promote")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Promote(Guid id)
    {
        var user = await _userRepository.GetByIdAsync(id);

        if (user is null)
            return NotFound();

        if (user.Role == RoleEnum.Admin)
            return BadRequest(new { Message = "Usuário já é Admin." });

        await _userRepository.PromoteToAdminAsync(id);

        return Ok(new { Message = "Usuário promovido para Admin com sucesso!" });
    }
}