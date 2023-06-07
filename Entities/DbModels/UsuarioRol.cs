using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tg_usuario_rol")]
    public class UsuarioRol
    {
        [Column("CRR_IDUSUARIO")]
        public int idUsuario { get; set; }

        [Required, Column("CRR_IDROL")]
        public int idRol { get; set; }


        [ForeignKey(nameof(idUsuario))]
        public UsuarioPortal usuario { get; set; }

        [ForeignKey(nameof(idRol))]
        public Rol rol { get; set; }
    }
}
