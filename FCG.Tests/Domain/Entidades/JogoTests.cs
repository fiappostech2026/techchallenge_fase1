using Xunit;

namespace FCG.Tests.Domain.Entidades
{
    public class JogoTests
    {
        [Fact]
        public void Promocao_PrecoPromocionalMaiorPrecoOriginal_Invalido()
        {
            //Arrange
            decimal preco = 100;
            decimal precoPromocional = 150;

            //Act
            bool isValid = precoPromocional < preco;

            //Assert
            Assert.False(isValid);
        }

        [Fact]
        public void Promocao_PrecoPromocionalMenorPrecoOriginal_Valido()
        {
            //Arrange
            decimal preco = 100;
            decimal precoPromocional = 80;

            //Act
            bool isValid = precoPromocional > 0 && precoPromocional < preco;

            //Assert
            Assert.True(isValid);
        }

        [Fact]
        public void Promocao_PrecoPromocional_ZeroOuNegativo_Invalido()
        {
            //Arrange
            decimal precoPromocional = 0;

            //Act
            bool isValid = precoPromocional > 0;

            //Assert
            Assert.False(isValid);
        }
    }
}