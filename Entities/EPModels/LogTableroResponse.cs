using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class LogTableroResponse
    {
        public int usuarioId { get; set; }
        public string nombreUsuario { get; set; }
        public int idCliente { get; set; }
        public string cargo { get; set; }
        public string correo { get; set; }
        public string zona { get; set; }
        public string urlServicio { get; set; }
        public int tipoNivelCliente { get; set; }
        public string nombreCliente { get; set; }

        public DateTimeOffset fecha { get; set; }
    }
}
