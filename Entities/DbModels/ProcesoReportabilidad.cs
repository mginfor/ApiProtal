using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class ProcesoReportabilidad
    {
        public string NOMBRE_CLIENTE { get; set; }
        public string NOMBRE_FAENA { get; set; }
        public string NOMBRE_CANDIDATO { get; set; }
        public int RUN_CANDIDATO { get; set; }
        public string DIG_CANDIDATO { get; set; }
        public string DESC_PERFIL { get; set; }
        public string NOMBRE_EVALUADOR { get; set; }
        public string PORCENTAJE_AVANCE { get; set; }
        public int COD_RESULTADO { get; set; }
        public DateTime? FECHA_SOCIALIZACION { get; set; }
        public DateTime? FECHA_ELEGIBILIDAD { get; set; }
        public DateTime? FECHA_REAL_PROT { get; set; }
        public DateTime? FECHA_REAL_PROT2 { get; set; }
        public DateTime? FECHA_REAL_PCT { get; set; }
        public DateTime? FECHA_REAL_EJD { get; set; }
        public DateTime? FECHA_EI { get; set; }
        public DateTime? FECHA_AUDITORIA { get; set; }
        public DateTime? FECHA_VALIDACION_CHILE_VALORA { get; set; }
        public DateTime? FECHA_INICIO { get; set; }
        public DateTime? FECHA_TERMINO { get; set; }
        public bool FLG_ELEGIBILIDAD { get; set; }
    }
}
