using FCG.Domain.Dto;
using FCG.Domain.Validators;
using Xunit;

namespace FCG.Tests.Domain.Validadores
{
    public class UsuarioValidatorTests
    {
        private readonly UsuarioValidator _validator = new();


        [Fact]
        public void Email_Vazio_Invalido()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "", Senha = "Senha@123" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public void Email_FormatoInvalido()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "nao-e-um-email", Senha = "Senha@123" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.PropertyName == "Email");
        }

        [Fact]
        public void Email_Valido()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "usuario@email.com", Senha = "Senha@123" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "Email");
        }


        [Fact]
        public void Senha_Vazia_Invalida()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "usuario@email.com", Senha = "" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.PropertyName == "Senha");
        }

        [Fact]
        public void Senha_MenosDeOitoCaracteres_Invalida()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "usuario@email.com", Senha = "Ab1@" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e =>
                e.PropertyName == "Senha" && e.ErrorMessage.Contains("8 caracteres"));
        }

        [Fact]
        public void Senha_SemLetra_Invalida()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "usuario@email.com", Senha = "12345678@" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e =>
                e.PropertyName == "Senha" && e.ErrorMessage.Contains("letra"));
        }

        [Fact]
        public void Senha_SemNumero_Invalida()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "usuario@email.com", Senha = "Senha@abc" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e =>
                e.PropertyName == "Senha" && e.ErrorMessage.Contains("número"));
        }

        [Fact]
        public void Senha_SemCaractereEspecial_Invalida()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "usuario@email.com", Senha = "Senha1234" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e =>
                e.PropertyName == "Senha" && e.ErrorMessage.Contains("especial"));
        }

        [Fact]
        public void Senha_Valida()
        {
            // Arrange
            var dto = new UsuarioDto { Email = "usuario@email.com", Senha = "Senha@123" };

            // Act
            var resultado = _validator.Validate(dto);

            // Assert
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "Senha");
        }
    }
}