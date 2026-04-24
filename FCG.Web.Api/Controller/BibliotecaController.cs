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

        public BibliotecaController(IBibliotecaRepository bibliotecaRepository, IJogoRepository jogoRepository, IBibliotecaService bibliotecaService)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _jogoRepository = jogoRepository;
            _bibliotecaService = bibliotecaService;
        }

        [HttpGet]
        public async Task<ActionResult> MinhaBiblioteca()
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

        [HttpPost("comprar/{jogoId:guid}")]
        public async Task<IActionResult> ComprarJogo(Guid jogoId)
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
    }
}