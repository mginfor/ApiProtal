using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    public class ProyectosyContratos
    {
        public string NOMBRE_CLIENTE { get; set; }
        public string GLS_CONTRATO { get; set; }
        public int crr_idproyecto_contrato { get; set; }
        public string gls_proyecto { get; set; }
    }
}
