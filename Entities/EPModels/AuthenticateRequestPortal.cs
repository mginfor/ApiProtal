using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class AuthenticateRequestPortal
    {
        [Required]
        public string RutUsuario { get; set; }

        [Required]
        public string RutCliente { get; set; }

        public int Codigo { get; set; }
    }
}
