using FCG.Domain.Entitie;
using FCG.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FCG.Infra.Context;

public class FcgContext : DbContext
{
    public FcgContext(DbContextOptions<FcgContext> options) : base(options) { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Jogo> Jogo { get; set; }
    public DbSet<Biblioteca> Biblioteca { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FcgContext).Assembly);
    }

}