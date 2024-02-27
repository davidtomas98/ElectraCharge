using System.ComponentModel.DataAnnotations.Schema;

namespace ElectraCharge.Models
{
    // Atributos para mapear la clase a la tabla "Usuarios" en la base de datos
    [Table("Usuarios")]
    public class Usuario
    {
        // Propiedad para el ID del usuario
        [Column("id_usuario")]
        public int Id { get; set; }

        // Propiedad para el nombre del usuario
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        // Propiedad para el primer apellido del usuario
        [Column("apellido1")]
        public string Apellido1 { get; set; } = string.Empty;

        // Propiedad para el segundo apellido del usuario
        [Column("apellido2")]
        public string Apellido2 { get; set; } = string.Empty;

        // Propiedad para el correo electrónico del usuario
        [Column("email")]
        public string CorreoElectronico { get; set; } = string.Empty;
        
        public virtual ICollection<Asignar>? Asignaciones { get; set; }
    }
}
