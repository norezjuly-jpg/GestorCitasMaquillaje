using GestorCitasMaquillaje.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorCitasMaquillaje.Infrastructure.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Maquillista> Maquillistas => Set<Maquillista>();
    public DbSet<Servicio> Servicios => Set<Servicio>();
    public DbSet<Cita> Citas => Set<Cita>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Servicio>()
            .Property(s => s.Precio).HasPrecision(10, 2);

        modelBuilder.Entity<Cita>()
            .Property(c => c.MontoTotal).HasPrecision(10, 2);

        modelBuilder.Entity<Cita>()
            .Property(c => c.MontoPagado).HasPrecision(10, 2);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Cliente)
            .WithMany(cl => cl.Citas)
            .HasForeignKey(c => c.ClienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Maquillista)
            .WithMany(m => m.Citas)
            .HasForeignKey(c => c.MaquillistaId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Cita>()
            .HasOne(c => c.Servicio)
            .WithMany(s => s.Citas)
            .HasForeignKey(c => c.ServicioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}