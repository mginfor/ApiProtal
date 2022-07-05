using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class ClienteEP
    {
        public int idCliente { get; set; }
        public string rut { get; set; }
        public string nombreCliente { get; set; }
        public string nombreFantasia { get; set; }
        public int? tipoNivelCliente { get; set; }


    }
}
