using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tg_rol")]
    public class Rol
    {
        [Key]
        [Column("CRR_IDROL")]
        public int id { get; set; }

        [Required,Column("NOMBRE_ROL")]
        public string NombreRol { get; set; }


    }
}
