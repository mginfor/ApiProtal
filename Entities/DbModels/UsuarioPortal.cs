using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tg_usuarios")]
    public class UsuarioPortal
    {
        [Key]
        [Column("CRR_IDUSUARIO")]
        public int id { get; set; }

        [Column("NOMBRE_USUARIO")]
        public string nombreUsuario { get; set; }

        [Column("APELLIDO_USUARIO")]
        public string apellidoUsuario { get; set; }

        [Column("RUN_USUARIO")]
        public string run { get; set; }

        [Column("CARGO_USUARIO")]
        public string cargo { get; set; }

        [Column("TELEFONO_USUARIO")]
        public int? telefono { get; set; }


        [Column("CRR_CLIENTE")]
        public int idCliente { get; set; }

        [Column("CORREO_USUARIO")]
        public string correo { get; set; }

        [Column("CLAVE_USUARIO")]
        public int clave { get; set; }

        [Column("ESTADO_USUARIO")]
        public int estado { get; set; }

        [Column("FECHA_BLOQ")]
        public DateTime? fechaBloqueo { get; set; }

        [Column("INTENTOS")]
        public int intentos { get; set; }

        [Column("ACTIVO")]
        public int activo { get; set; }

        [Column("ZONA_FAENA")]
        public string zona { get; set; }

        [Column("CRR_IDROL")]
        public int idRol { get; set; }

        [Column("FECHA_CODIGO")]
        public DateTime? UltimoEnvioCodigo { get; set; }

        [Column("PASS")]
        public string Pass { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();

        //FKs
        [ForeignKey("idCliente")]
        public Cliente cliente { get; set; }

        [ForeignKey("idRol")]
        public Rol rol { get; set; }





    }
}
