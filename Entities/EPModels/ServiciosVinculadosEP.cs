using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
   public class ServiciosVinculadosEP
    {
        public int idCliente { get; set; }
        public int idServicios { get; set; }
        public string urlServicio { get; set; }
        public bool estado { get; set; }
    }
}
