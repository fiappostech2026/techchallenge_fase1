using FCG.Domain.Entitie;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FCG.Infra.Config;

public class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Password)
            .IsRequired()
            .HasMaxLength(255);
        
        builder.Property(x => x.Role)
            .IsRequired()
            .HasConversion<string>(); 

        // Relacionamento com User
        // builder.HasOne(x => x.User)
        //     .WithMany(x => x.UserGames)
        //     .HasForeignKey(x => x.UserId);
    }
}