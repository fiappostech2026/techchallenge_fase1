using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infra.Config
{
    public class BibliotecaConfig : IEntityTypeConfiguration<Biblioteca>
    {
        public void Configure(EntityTypeBuilder<Biblioteca> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.DataCompra)
                .IsRequired();

            builder.Property(b => b.PrecoPago)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(b => b.UsuarioId)
                .IsRequired();

            builder.Property(b => b.JogoId)
                .IsRequired();

            builder
                .HasOne<Usuario>()
                .WithMany()
                .HasForeignKey(b => b.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne<Jogo>()
                .WithMany()
                .HasForeignKey(b => b.JogoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(b => new { b.UsuarioId, b.JogoId })
                .IsUnique();
        }
    }
}