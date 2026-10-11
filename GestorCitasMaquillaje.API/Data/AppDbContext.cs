using Microsoft.EntityFrameworkCore;
using GestorCitasMaquillaje.API.Models;

namespace GestorCitasMaquillaje.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Maquillista> Maquillistas { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Cita> Citas { get; set; }
    }
}