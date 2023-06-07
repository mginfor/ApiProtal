using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tg_permiso")]
    public class Permisos
    {
        [Key]
        [Column("CRR_IDPERMISO")]
        public int id { get; set; }

        [Required,Column("NOMBRE_PERMISO")]
        public string NombrePermiso { get; set; }
    }
}
