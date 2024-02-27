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

        // DbSet para la clase Charger
        public DbSet<Cargador> Cargadores { get; set; }

        // DbSet para la clase Administrador
        public DbSet<Administrador> Administradores { get; set; }

        // DbSet para la clase Asignar
        public DbSet<Asignar> Asignaciones { get; set; }

       protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asignar>()
                .HasKey(a => a.Id);

            modelBuilder.Entity<Asignar>()
                .HasOne(a => a.Usuario)
                .WithMany()
                .HasForeignKey(a => a.IdUsuario);

            modelBuilder.Entity<Asignar>()
                .HasOne(a => a.Cargador)
                .WithMany()
                .HasForeignKey(a => a.IdCargador);
        }


    }
}
