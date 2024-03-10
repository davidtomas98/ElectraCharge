using System.ComponentModel.DataAnnotations.Schema;

namespace ElectraCharge.Models
{
    // Atributos para mapear la clase a la tabla "Cargadores" en la base de datos
    [Table("Cargadores")]
    public class Cargador
    {
        // Propiedad para el ID del cargador
        [Column("id_cargador")]
        public int Id { get; set; }

        // Propiedad para la marca del cargador
        [Column("marca")]
        public string Marca { get; set; } = string.Empty;

        // Propiedad para la ubicación del cargador
        [Column("ubicacion")]
        public string Ubicacion { get; set; } = string.Empty;

        // Propiedad para la potencia del cargador
        [Column("potencia")]
        public int Potencia { get; set; }

        // Propiedad para el estado del cargador
        [Column("estado")]
        public string Estado { get; set; } = string.Empty;

        // Constructor para inicializar el estado del cargador como "Disponible" por defecto
        public Cargador()
        {
            Estado = "Disponible";
        }

    }
}
