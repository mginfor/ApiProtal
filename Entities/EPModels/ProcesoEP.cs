    using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class ProcesoEP
    {

        public string idCliente { get; set; }
        public string? runCandidato { get; set; }
        public string dni_Pasaporte { get; set; }
        public string? estado { get; set; }
        public string? vigencia { get; set; }
        public string? resultado { get; set; }
        public string? fechaInicio { get; set; }
        public string? fechaFinal { get; set; }
        public string? zonaFaena { get; set; }

    }
}
