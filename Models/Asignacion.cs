using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectraCharge.Models
{
    [Table("Asignaciones")]
    public class Asignacion
    {
        [Column("id_asignar")]
        public int Id { get; set; }

        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [ForeignKey("IdUsuario")]
        public Usuario? Usuario { get; set; }

        [Column("id_cargador")]
        public int IdCargador { get; set; }

        [ForeignKey("IdCargador")]
        public Cargador? Cargador { get; set; }

        [Column("tiempo")]
        public TimeSpan Tiempo { get; set; }

        [Column("fecha")]
        public DateTime Fecha { get; set; }
    }
}
