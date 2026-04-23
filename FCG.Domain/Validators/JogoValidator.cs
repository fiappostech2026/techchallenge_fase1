using FCG.Domain.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Domain.Validators
{
    public class JogoDtoValidator : AbstractValidator<JogoDto>
    {
        public JogoDtoValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty()
                .WithMessage("Nome é obrigatório");

            RuleFor(x => x.Preco)
                .GreaterThan(0)
                .WithMessage("Preço deve ser maior que zero");

            //RuleFor(x => x.DataLancamento)
            //    .LessThanOrEqualTo(DateTime.Now)
            //    .WithMessage("Data inválida");
        }
    }
}
