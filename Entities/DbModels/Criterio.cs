using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tg_criterio")]
    public class Criterios
    {
        [Key]
        [Column("CRR_IDCRITERIO")]
        public int IdCriterio { get; set; }

        [Column("COD_CRITERIO")]
        public string CodigoCriterio { get; set; }

        [Column("GLS_CRITERIO")]
        public string DescripcionCriterio { get; set; }

        [Column("COD_CLAVE")]
        public string CodigoClave { get; set; }

        [Column("COD_UCL")]
        public string CodigoUCL { get; set; }

        [Column("CRR_UCL")]
        public int? UCL { get; set; }

        [Column("CRR_ACTIVIDAD")]
        public int? IdActividad { get; set; }

        [Column("FLG_VIGENCIA")]
        public bool? Vigencia { get; set; }
    }
}
