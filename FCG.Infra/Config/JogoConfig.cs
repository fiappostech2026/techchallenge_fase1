using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FCG.Infra.Config
{
    public class JogoConfig : IEntityTypeConfiguration<Jogo>
    {
        public void Configure(EntityTypeBuilder<Jogo> builder)
        {
            builder.ToTable("Jogo");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .IsRequired();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(400);

            builder.Property(x => x.Preco)
                .HasColumnType("decimal(10,2)")
                .IsRequired();

            builder.Property(x => x.PrecoPromocional)
                .HasColumnType("decimal(10,2)");

            builder.Property(x => x.Genero)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.DataLancamento)
                .IsRequired();

            builder.Property(x => x.Plataforma)
                .IsRequired()
                .HasConversion<string>();
        }
    }
}