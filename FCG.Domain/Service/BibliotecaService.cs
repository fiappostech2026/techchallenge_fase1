using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Domain.Interfaces.IService;

namespace FCG.Domain.Service
{
    public class BibliotecaService : IBibliotecaService
    {
        private readonly IBibliotecaRepository _bibliotecaRepository;
        private readonly IJogoRepository _jogoRepository;

        public BibliotecaService(IBibliotecaRepository bibliotecaRepository, IJogoRepository jogoRepository)
        {
            _bibliotecaRepository = bibliotecaRepository;
            _jogoRepository = jogoRepository;
        }

        public async Task<bool> ComprarJogo(Guid usuarioId, Guid jogoId)
        {
            var jogo = await _jogoRepository.ObterPorIdAsync(jogoId);

            if (jogo is null)
                return false;

            var jaComprou = await _bibliotecaRepository.UsuarioJaPossuiJogo(usuarioId, jogoId);

            if (jaComprou)
                return false;

            var precoFinal = jogo.PrecoPromocional ?? jogo.Preco;

            var biblioteca = new Biblioteca
            {
                UsuarioId = usuarioId,
                JogoId = jogoId,
                DataCompra = DateTime.Now,
                PrecoPago = precoFinal
            };

            await _bibliotecaRepository.AdicionarAsync(biblioteca);
            await _bibliotecaRepository.SalvarAlteracoesAsync();

            return true;
        }
    }
}