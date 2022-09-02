using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class FaenaEP
    {
        public int id { get; set; }

        public string descripcion { get; set; }

        public int vigencia { get; set; }

        public string zona { get; set; }

        public FaenaEP(int id, string descripcion, int vigencia, string zona)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.vigencia = vigencia;
            this.zona = zona;
        }
    }
}
