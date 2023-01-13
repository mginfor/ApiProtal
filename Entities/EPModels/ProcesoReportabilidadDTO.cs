using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class ProcesoReportabilidadDTO
    {
            public string Empresa { get; set; }
            public string Faena { get; set; }
            public string NombreTrabajador { get; set; }
            public string Rut { get; set; }
            public string Perfil { get; set; }
            public string Evaluador { get; set; }
            public DateTime? Socializacion { get; set; }
            public DateTime? Elegibilidad { get; set; }
            public DateTime? PruebaTeorica { get; set; }
            public DateTime? EvidenciaDocumental { get; set; }
            public DateTime? EvaluacionPractica1 { get; set; }
            public DateTime? EvaluacionPractica2 { get; set; }
            public DateTime? CuestionarioJefaturaDirecta { get; set; }
            public DateTime? Auditoria { get; set; }
            public DateTime? ProcesoValidacionChilevalora { get; set; }
            public string ResultadoChilevalora { get; set; }
            public string ProcentajeAvance { get; set; }
            public DateTime? FECHAINICIO { get; set; }
            public DateTime? FECHATERMINO { get; set; }
            public bool EsElegible{ get; set; }

    }
}
