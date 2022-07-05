using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class GraficoCriterio
    {
        public string codCriterio { get; set; }
        public string codActividad { get; set; }
        public int correctas { get; set; }
        public int incorrectas { get; set; }
        public int total { get; set; }
        public double cumplimientoIndustria { get; set; }
    }
}
