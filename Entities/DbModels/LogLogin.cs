using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("log_login")]
    public class LogLogin
    {
        [Column("CRR_IDLOG")]
        [Key]
        public int crr_idLog { get; set; }

        [Column("NOMBRE_USUARIO")]
        public string nombreUsuario { get; set; }

        [Column("CARGO_USUARIO")]
        public string cargoUsuario { get; set; }


        [Column("CRR_IDUSUARIO")]
        public int id_usuario { get; set; }
        [Column("TOKEN")]
        public string token { get; set; }
        [Column("FECHA_CREACION")]
        public DateTime fecha_creacion { get; set; }
        [Column("FECHA_EXPIRACION")]
        public DateTime fecha_expiracion { get; set; }

        //FKs
        [ForeignKey("id_usuario")]
        public UsuarioPortal usuario { get; set; }


    }
}
