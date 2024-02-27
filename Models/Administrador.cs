using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectraCharge.Models
{
    // Atributos para mapear la clase a la tabla "Administradores" en la base de datos
    [Table("Administradores")]
    public class Administrador
    {
        // Propiedad para el ID del administrador
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id_admin")]
        public int Id { get; set; }

        // Propiedad para el nombre de usuario del administrador
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre de usuario debe tener como máximo {1} caracteres.")]
        [Column("nombre")]
        public string Username { get; set; } = string.Empty;

        // Propiedad para el correo electrónico del administrador
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        [StringLength(256, ErrorMessage = "El correo electrónico debe tener como máximo {1} caracteres.")]
        [Column("email")]
        public string Email { get; set; } = string.Empty;

        // Propiedad para la contraseña del administrador
        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "La contraseña debe tener como mínimo {2} y como máximo {1} caracteres.", MinimumLength = 6)]
        [Column("contraseña")]
        public string Password { get; set; } = string.Empty;
    }
}
