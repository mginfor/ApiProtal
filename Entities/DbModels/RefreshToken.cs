using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        [Key]
        [Column("CRR_IDTOKEN")]
        public int? id { get; set; }

        [Column("CRR_IDUSUARIO")]
        public int idUsuario { get; set; }

        [Column("TOKEN")]
        public string Token { get; set; }

        [Column("EXPIRES")]
        public DateTime Expires { get; set; }

        public bool IsExpired => DateTime.UtcNow >= Expires;

        [Column("CREATED")]
        public DateTime Created { get; set; }

        [Column("REVOKED")]
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        //FKs
        [ForeignKey("idUsuario")]
        public UsuarioPortal Usuario { get; set; }


    }
}



