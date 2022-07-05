using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("log")]
    public class Log
    {
        [Key]
        [Column("CRR_IDLOG")]
        public int id { get; set; }

        [Column("FECHAIN")]
        public DateTime fechaIngreso { get; set; }

        [Column("FECHAOUT")]
        public DateTime? fechaSalida { get; set; }

        [Column("IP")]
        public string? ip { get; set; }

        [Column("UBICACION")]
        public string? ubicacion { get; set; }

        [Column("NOMBRE_EQUIPO")]
        public string? nombreEquipo { get; set; }

        [Column("CLAVE")]
        public string clave { get; set; }

        [Column("COMENTARIO")]
        public string? comentario { get; set; }

        [Column("CRR_USUARIO")]
        public int idUsuario { get; set; }

        //FKs
        [ForeignKey("idUsuario")]
        public UsuarioPortal usuarioPortal { get; set; }
        
    }
}
