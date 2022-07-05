
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tg_estado_proceso")] 
    public class EstadoProceso
    {
        [Key]
        [Column("CRR_IDESTADO_PROCESO")]
        public int id { get; set; }

        [Column("GLS_ESTADO")]
        public string Descripcion { get; set; }
    }
}
