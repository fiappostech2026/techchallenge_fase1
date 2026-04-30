using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Domain.Interfaces.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FCG.Controller
{
    [ApiController]
    [Route("api/v1/biblioteca")]
    [Authorize]
    public class BibliotecaController : ControllerBase
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        private readonly IJogoRepository _jogoRepository;
        private readonly IBibliotecaService _bibliotecaService;
        private readonly ILogger<BibliotecaController> _logger;

        public BibliotecaController(IBibliotecaRepository bibliotecaRepository, IJogoRepository jogoRepository, IBibliotecaService bibliotecaService, ILogger<BibliotecaController> logger)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _jogoRepository = jogoRepository;
            _bibliotecaService = bibliotecaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> MinhaBiblioteca()
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (usuarioId is null)
                    return Unauthorized();

                var usuarioGuid = Guid.Parse(usuarioId);

                var biblioteca = await _bibliotecaRepository.ObterPorIdAsync(usuarioGuid);

                if (biblioteca is null)
                    return NotFound();

                return Ok(biblioteca);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar biblioteca do usuário.");
                throw;
            }
        }

        [HttpPost("comprar/{jogoId:guid}")]
        public async Task<IActionResult> ComprarJogo(Guid jogoId)
        {
            try
            {
                var usuarioId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (usuarioId is null)
                    return Unauthorized();

                var usuarioGuid = Guid.Parse(usuarioId);

                var compraRealizada = await _bibliotecaService.ComprarJogo(usuarioGuid, jogoId);

                if (!compraRealizada)
                    return BadRequest(new { Mensagem = "Não foi possível realizar a compra." });

                return Ok(new { Mensagem = "Compra realizada com sucesso!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao comprar jogo {JogoId}.", jogoId);
                throw;
            }
        }
    }
}
