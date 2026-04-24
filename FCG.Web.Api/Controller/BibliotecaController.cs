using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Domain.Interfaces.IService;
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

            var compraRealizada = await _bibliotecaService.ComprarJogo(userGuid, jogoId);

            if (!compraRealizada)
                return BadRequest(new { Message = "Não foi possível realizar a compra." });

            return Ok(new { Message = "Compra realizada com sucesso!" });
        }
    }
}
