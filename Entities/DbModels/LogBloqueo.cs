using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("LogBloqueos")]
    public class LogBloqueo
    {
        [Key]
        public int Id { get; set; }

        [Column("IdUsuario")]
        public int IdUsuario { get; set; }

        [Column("NombreUsuario")]
        public string NombreUsuario { get; set; }

        [Column("CargoUsuario")]
        public string CargoUsuario { get; set; }

        [Column("CRR_CLIENTE")]
        public int CrrCliente { get; set; }

        [Column("FechaBloqueo")]
        public DateTime FechaBloqueo { get; set; }

        [Column("IP")]
        public string DireccionIP { get; set; }

        // Relaciones
        [ForeignKey("IdUsuario")]
        public virtual UsuarioPortal Usuario { get; set; }
    }

}
