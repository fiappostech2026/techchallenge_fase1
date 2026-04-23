using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Domain.Entities
{
    public class Jogo
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public decimal? PrecoPromocional { get; set; }
        public string Genero { get; set; } = string.Empty;
        public DateTime DataLancamento { get; set; }
        public string Plataforma { get; set; } = string.Empty;
    }
}
