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
        private readonly ILogger<JogoController> _logger;

        public JogoController(IJogoRepository jogoRepository, IValidator<JogoDto> validator, ILogger<JogoController> logger)
        {
            _jogoRepository = jogoRepository;
            _validator = validator;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> RecuperarPorId(Guid id)
        {
            try
            {
                var jogo = await _jogoRepository.ObterPorIdAsync(id);

                if (jogo is null)
                    return NotFound();

                return Ok(jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar jogo {JogoId}.", id);
                throw;
            }
        }

        [HttpGet]
        public async Task<ActionResult> ObterTodos()
        {
            try
            {
                var jogos = await _jogoRepository.ObterTodosAsync();
                return Ok(jogos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar lista de jogos.");
                throw;
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Criar([FromBody] JogoDto dto)
        {
            try
            {
                var validacao = await _validator.ValidateAsync(dto);

                if (!validacao.IsValid)
                    return BadRequest(validacao.Errors.Select(e => e.ErrorMessage));

                var jogo = new Jogo
                {
                    Nome = dto.Nome,
                    Descricao = dto.Descricao,
                    Preco = dto.Preco,
                    Genero = dto.Genero,
                    DataLancamento = dto.DataLancamento,
                    Plataforma = dto.Plataforma
                };

                await _jogoRepository.AdicionarAsync(jogo);
                await _jogoRepository.SalvarAlteracoesAsync();

                return CreatedAtAction(nameof(RecuperarPorId), new { id = jogo.Id }, jogo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar jogo {NomeJogo}.", dto.Nome);
                throw;
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] JogoDto dto)
        {
            try
            {
                var validacao = await _validator.ValidateAsync(dto);

                if (!validacao.IsValid)
                    return BadRequest(validacao.Errors.Select(e => e.ErrorMessage));

                var jogoExistente = await _jogoRepository.ObterPorIdAsync(id);

                if (jogoExistente == null)
                    return NotFound();

                jogoExistente.Nome = dto.Nome;
                jogoExistente.Descricao = dto.Descricao;
                jogoExistente.Preco = dto.Preco;
                jogoExistente.Genero = dto.Genero;
                jogoExistente.DataLancamento = dto.DataLancamento;
                jogoExistente.Plataforma = dto.Plataforma;

                _jogoRepository.Atualizar(jogoExistente);
                await _jogoRepository.SalvarAlteracoesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar jogo {JogoId}.", id);
                throw;
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Excluir(Guid id)
        {
            try
            {
                var jogo = await _jogoRepository.ObterPorIdAsync(id);

                if (jogo == null)
                    return NotFound();

                _jogoRepository.Remover(jogo);
                await _jogoRepository.SalvarAlteracoesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao excluir jogo {JogoId}.", id);
                throw;
            }
        }

        [HttpPatch("{id:guid}/promocao")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CriarPromocao(Guid id, [FromBody] PromocaoDto promocaoDto)
        {
            try
            {
                var jogo = await _jogoRepository.ObterPorIdAsync(id);

                if (jogo == null)
                    return NotFound();

                if (promocaoDto.PrecoPromocional <= 0 || promocaoDto.PrecoPromocional >= jogo.Preco)
                    return BadRequest("Preço promocional deve ser maior que 0 e menor que o preço original.");

                jogo.PrecoPromocional = promocaoDto.PrecoPromocional;

                _jogoRepository.Atualizar(jogo);
                await _jogoRepository.SalvarAlteracoesAsync();

                return Ok(new { Mensagem = "Promoção criada com sucesso!", jogo });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar promoção para jogo {JogoId}.", id);
                throw;
            }
        }
    }
}
