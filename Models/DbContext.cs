using Microsoft.EntityFrameworkCore;

namespace ElectraCharge.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSet para la clase Usuario
        public DbSet<Usuario> Usuarios { get; set; }

        // DbSet para la clase Charger
        public DbSet<Charger> Chargers { get; set; }

        // DbSet para la clase Administrador
        public DbSet<Administrador> Administradores { get; set; }
    }
}
