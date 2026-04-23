using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Infra.Repository;
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

        public BibliotecaController(IBibliotecaRepository bibliotecaRepository, IJogoRepository jogoRepository)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _jogoRepository = jogoRepository;
        }

        [HttpGet]        
        public async Task<ActionResult> MinhaBiblioteca()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                return Unauthorized();

            var userGuid = Guid.Parse(userId);

            var biblioteca = await _bibliotecaRepository.GetByIdAsync(userGuid);

            if (biblioteca is null)
                return NotFound();

            return Ok(biblioteca);
        }

        [HttpPost("comprar/{jogoId:guid}")]
        public async Task<IActionResult> ComprarJogo(Guid jogoId) 
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId is null)
                return Unauthorized();

            var userGuid = Guid.Parse(userId);

            var jogo = await _jogoRepository.GetByIdAsync(jogoId);

            if (jogo is null)
                return NotFound(new { Message = "Jogo não encontrado" });

            var jaComprou = await _bibliotecaRepository.UsuarioJaPossuiJogo(userGuid, jogoId);

            if (jaComprou)
                return BadRequest(new { Message = "Jogo já adquirido" });

            var precoFinal = jogo.PrecoPromocional ?? jogo.Preco;

            var biblioteca = new Biblioteca
            {
                UserId = userGuid,
                JogoId = jogoId,
                DataCompra = DateTime.Now,
                PrecoPago = precoFinal
            };

            await _bibliotecaRepository.AddAsync(biblioteca);
            await _bibliotecaRepository.SaveChangesAsync();

            return Ok(new { Message = "Compra realizada com sucesso!" });

        }
    }
}
