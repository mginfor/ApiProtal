using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class GraficoTablero
    {
        public string NombreFaena { get; set; }
        public List<int> cantidadProcesos { get; set; }
        public List<double> porcentajeCumplimiento { get; set; }
    }
}
