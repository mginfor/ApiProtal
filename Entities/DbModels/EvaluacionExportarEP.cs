using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class EvaluacionExportarEP
    {
        public int id { get; set; }
        public int? idPerfil { get; set; }
        public int? idCliente { get; set; }
        public int? idFaena { get; set; }
    }
}
