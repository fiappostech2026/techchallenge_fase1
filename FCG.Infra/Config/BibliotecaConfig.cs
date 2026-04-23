using FCG.Domain.Entitie;
using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

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

            builder.Property(b => b.UserId)
                .IsRequired();

            builder.Property(b => b.JogoId)
                .IsRequired();

            builder
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne<Jogo>()
                .WithMany()
                .HasForeignKey(b => b.JogoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasIndex(b => new { b.UserId, b.JogoId })
                .IsUnique();
        }
    }
}
