using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Domain.Dto
{
    public class JogoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Genero { get; set; } = string.Empty;
        public DateTime DataLancamento { get; set; }
        public string Plataforma { get; set; } = string.Empty;
    }
}
