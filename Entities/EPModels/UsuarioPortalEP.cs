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

        public string cargo { get; set; }

        public string zona { get; set; }
        public string UrlServicio { get; set; }

        public string nombrecliente { get; set; }

        public int? tipoNivelCliente { get; set; }

        public Rol rol { get; set; }

        public List<Permisos> permisos { get; set; }





    }
}
