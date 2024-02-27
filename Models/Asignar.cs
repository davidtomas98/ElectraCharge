using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectraCharge.Models
{
    [Table("Asignaciones")]
    public class Asignar
    {
        [Key]
        [Column("id_asignar")]
        public int Id { get; set; }

        // Propiedad de navegación para Usuario
        [ForeignKey("IdUsuario")]

        [Column("id_usuario")]
        public int? IdUsuario { get; set; }
        public Usuario? Usuario { get; set; }

        // Propiedad de navegación para Cargador
        [ForeignKey("IdCargador")]

        [Column("id_cargador")]
        public int? IdCargador { get; set; }
        public Cargador? Cargador { get; set; }

        [Column("tiempo")]
        public DateTime Tiempo { get; set; }

        public virtual ICollection<Usuario>? Usuarios { get; set; }
        public virtual ICollection<Cargador>? Cargadores { get; set; }
    }
}
