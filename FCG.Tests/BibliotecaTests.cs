using FCG.Domain.Entities;
using FCG.Domain.Interfaces.IRepository;
using FCG.Domain.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FCG.Tests
{
    public class BibliotecaTests
    {
        [Fact]
        public async Task ComprarJogo_JogoInexistente_RetornaFalse()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();

            var mockBibliotecaRepo = new Mock<IBibliotecaRepository>();
            var mockJogoRepo = new Mock<IJogoRepository>();

            mockJogoRepo
                .Setup(x => x.ObterPorIdAsync(jogoId))
                .ReturnsAsync((Jogo)null);

            var service = new BibliotecaService(
                mockBibliotecaRepo.Object,
                mockJogoRepo.Object
            );

            // Act
            var resultado = await service.ComprarJogo(usuarioId, jogoId);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task ComprarJogo_JogoJaComprado_RetornaFalse()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();

            var mockBibliotecaRepo = new Mock<IBibliotecaRepository>();
            var mockJogoRepo = new Mock<IJogoRepository>();

            var jogo = new Jogo
            {
                Id = jogoId,
                Preco = 100
            };

            mockJogoRepo
                .Setup(x => x.ObterPorIdAsync(jogoId))
                .ReturnsAsync(jogo);

            mockBibliotecaRepo
                .Setup(x => x.UsuarioJaPossuiJogo(usuarioId, jogoId))
                .ReturnsAsync(true);

            var service = new BibliotecaService(mockBibliotecaRepo.Object, mockJogoRepo.Object);

            // Act
            var resultado = await service.ComprarJogo(usuarioId, jogoId);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task ComprarJogo_CompraBemSucedida_RetornaTrueAsync()
        {
            // Arrange
            var usuarioId = Guid.NewGuid();
            var jogoId = Guid.NewGuid();

            var mockBibliotecaRepo = new Mock<IBibliotecaRepository>();
            var mockJogoRepo = new Mock<IJogoRepository>();

            var jogo = new Jogo
            {
                Id = jogoId,
                Preco = 100
            };

            mockJogoRepo
                .Setup(x => x.ObterPorIdAsync(jogoId))
                .ReturnsAsync(jogo);

            mockBibliotecaRepo
                .Setup(x => x.UsuarioJaPossuiJogo(usuarioId, jogoId))
                .ReturnsAsync(false);

            var service = new BibliotecaService(mockBibliotecaRepo.Object, mockJogoRepo.Object);

            // Act
            var resultado = await service.ComprarJogo(usuarioId, jogoId);

            // Assert
            Assert.True(resultado);
        }
    }
}