using FCG.Domain.Dto;
using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FCG.Controller
{
    [ApiController]
    [Route("api/v1/jogos")]
    [Authorize]
    public class JogoController : ControllerBase
    {
        private readonly IJogoRepository _jogoRepository;
        private readonly IValidator<JogoDto> _validator;

        public JogoController(IJogoRepository jogoRepository, IValidator<JogoDto> validator)
        {
            _jogoRepository = jogoRepository;
            _validator = validator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorId(Guid id)
        {
            var jogo = await _jogoRepository.GetByIdAsync(id);

            if (jogo is null)
                return NotFound();

            return Ok(jogo);
        }

        [HttpGet]
        public async Task<ActionResult> ObterTodos()
        {
            var jogos = await _jogoRepository.GetAllAsync();
            return Ok(jogos);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Criar([FromBody] JogoDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var jogo = new Jogo
            {
                Nome = dto.Nome,
                Descricao = dto.Descricao,
                Preco = dto.Preco,
                Genero = dto.Genero,
                DataLancamento = dto.DataLancamento,
                Plataforma = dto.Plataforma
            };

            await _jogoRepository.AddAsync(jogo);
            await _jogoRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(RecuperarPorId), new { id = jogo.Id }, jogo);
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Update(Guid id, [FromBody] JogoDto dto)
        {
            var validation = await _validator.ValidateAsync(dto);

            if (!validation.IsValid)
                return BadRequest(validation.Errors.Select(e => e.ErrorMessage));

            var jogoExistente = await _jogoRepository.GetByIdAsync(id);

            if (jogoExistente == null)
                return NotFound();

            jogoExistente.Nome = dto.Nome;
            jogoExistente.Descricao = dto.Descricao;
            jogoExistente.Preco = dto.Preco;
            jogoExistente.Genero = dto.Genero;
            jogoExistente.DataLancamento = dto.DataLancamento;
            jogoExistente.Plataforma = dto.Plataforma;

            _jogoRepository.Update(jogoExistente);
            await _jogoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var jogo = await _jogoRepository.GetByIdAsync(id);

            if (jogo == null)
                return NotFound();

            _jogoRepository.Remove(jogo);
            await _jogoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{id:guid}/promocao")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CriarPromocao(Guid id, [FromBody]PromocaoDto promocaoDto)
        {
            var jogo = await _jogoRepository.GetByIdAsync(id);

            if (jogo == null)
                return NotFound();

            if (promocaoDto.PrecoPromocional <= 0 || promocaoDto.PrecoPromocional >= jogo.Preco)
                return BadRequest("Preço promocional deve ser maior que 0 e menor que o preço original.");

            jogo.PrecoPromocional = promocaoDto.PrecoPromocional;

            _jogoRepository.Update(jogo);
            await _jogoRepository.SaveChangesAsync();

            return Ok(new { Message = "Promoção criada com sucesso!", jogo });
        }
    }
}
