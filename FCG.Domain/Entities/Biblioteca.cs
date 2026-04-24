namespace FCG.Domain.Entities
{
    public class Biblioteca
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime DataCompra { get; set; }
        public decimal PrecoPago { get; set; }
        public Guid UsuarioId { get; set; }
        public Guid JogoId { get; set; }
    }
}