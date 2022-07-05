using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class Tablero
    {
        public string Cliente { get; set; }
        public int Vigentes { get; set; }
        public int Historicos { get; set; }
        public int Total { get; set; }
        public double LogroEmpresa { get; set; }
        public double LogroIndustria { get; set; }
        public int CantidadPerfiles { get; set; }
        public int CantidadPersonas { get; set; }
        public int Acreditados { get; set; }
        public int NoAcreditados { get; set; }
        public int AcreditadosRef { get; set; }
        public int NoAcreditadosRef { get; set; }
        public List<string> Perfiles { get; set; }
        public List<GraficoTablero> DataGrafico { get; set; }
        public List<GraficoTablero> GraficoResumen { get; set; }
    }
}
