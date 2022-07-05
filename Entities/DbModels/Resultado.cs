using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tg_resultado")]
    public class Resultado
    {
        [Key]
        [Column("COD_RESULTADO")]
        public int id { get; set; }

        [Column("GLS_RESULTADO")]
        public string descripcion { get; set; }
    }
}
