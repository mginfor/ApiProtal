using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tg_faena")]
    public class Faena
    {
        [Key]
        [Column("CRR_IDFAENA")]
        public int id { get; set; }

        [Column("GLS_FAENA")]
        public string descripcion { get; set; }

        [Column("MESES_VIGENCIA")]
        public int vigencia { get; set; }

        [Column("ZONA_FAENA")]
        public string zona { get; set; }
    }
}
