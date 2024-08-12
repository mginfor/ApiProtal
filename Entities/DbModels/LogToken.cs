using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("log_loginToken")]
    public class LogToken
    {
        [Key]
        [Column("CRR_IDTOKEN")]
        public int? id { get; set; }

        [Column("CRR_IDUSUARIO")]
        public int idUsuario { get; set; }

        [Column("TOKEN")]
        public string Token { get; set; }

        [Column("FECHAINGRESO")]
        public DateTime FechaIngreso { get; set; }

        [Column("FECHAEXPIRACION")]
        public DateTime FechaExpiracion { get; set; }


        [Column("Revocado")]
        public bool Revocado { get; set; }

        [Column("ISVALID")]
        public bool IsValid { get => !Revocado && DateTime.UtcNow < FechaExpiracion; }

        //FKs
        [ForeignKey("idUsuario")]
        public UsuarioPortal Usuario { get; set; }


    }
}

