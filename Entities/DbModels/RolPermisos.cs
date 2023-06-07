using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tg_rol_permiso")]
    [Keyless]
    public class RolPermisos
    {
        [Column("CRR_IDROL")]
        public int idRol { get; set; }

        [Required, Column("CRR_IDPERMISO")]
        public int idPermiso { get; set; }


        [ForeignKey(nameof(idRol))]
        public Rol rol { get; set; }

        [ForeignKey(nameof(idPermiso))]
        public Permisos permiso { get; set; }
    }
}
