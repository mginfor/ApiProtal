using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class LevantamientoMap
    {
        public int idEvaluacion { get; set; }
        public string nombreArchivo{ get; set; }
        public string tratamiento { get; set; }
        public string entidad { get; set; }
        public string rutaDocumento { get; set; }
        public List<BrechaLevantamiento> brechas { get; set; }

    }

    public class BrechaLevantamiento
    {
        public string tipoBrecha { get; set; }
        public int idBrecha { get; set; }
    }

    public class InformeLevantamiento
    {
        public int idEvaluacion { get; set; }
        public string htmlDocument { get; set; }
    }
}
