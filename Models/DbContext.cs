using Microsoft.EntityFrameworkCore;

namespace ElectraCharge.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet para la clase Usuario
        public DbSet<Usuario> Usuarios { get; set; }

        // DbSet para la clase Cargador
        public DbSet<Cargador> Cargadores { get; set; }

        // DbSet para la clase Administrador
        public DbSet<Administrador> Administradores { get; set; }

        // DbSet para la clase Asignacion
        public DbSet<Asignacion> Asignaciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de las relaciones
            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.IdUsuario);

            modelBuilder.Entity<Asignacion>()
                .HasOne(a => a.Cargador)
                .WithMany()
                .HasForeignKey(a => a.IdCargador);
        }
    }
}
