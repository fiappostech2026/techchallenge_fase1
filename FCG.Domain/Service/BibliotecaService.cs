using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Domain.Interfaces.IService;
using System;
using System.Collections.Generic;
using System.Text;

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

        public async Task<bool> ComprarJogo(Guid userId, Guid jogoId)
        {
            var jogo = await _jogoRepository.GetByIdAsync(jogoId);

            if (jogo is null)
                return false;

            var jaComprou = await _bibliotecaRepository.UsuarioJaPossuiJogo(userId, jogoId);

            if (jaComprou)
                return false;

            var precoFinal = jogo.PrecoPromocional ?? jogo.Preco;

            var biblioteca = new Biblioteca
            {
                UserId = userId,
                JogoId = jogoId,
                DataCompra = DateTime.Now,
                PrecoPago = precoFinal
            };

            await _bibliotecaRepository.AddAsync(biblioteca);
            await _bibliotecaRepository.SaveChangesAsync();

            return true;
        }
    }
}
