using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class UsuarioPortalEP
    {
        public int id { get; set; }

        public string nombreUsuario { get; set; }

        public int idCliente { get; set; }


        public string correo { get; set; }

        //public int clave { get; set; }

        //public int estado { get; set; }
       // public DateTime? fechaBloqueo { get; set; }

        //public int intentos { get; set; }

       //public int activo { get; set; }

        public string cargo { get; set; }

        public string zona { get; set; }
        public string UrlServicio { get; set; }
        // 09.05.22 MTO
        // tipo nombre cliente
        public string nombrecliente { get; set; }

        //11.04.22 GCS
        // tipo nivel cliente
        public int? tipoNivelCliente { get; set; }


    }
}
