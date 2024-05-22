using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{

    [Table("Log_Tablero")]
    public class Log_Tablero
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int IdCliente { get; set; }
        public string Cargo { get; set; }
        public string Correo { get; set; }
        public string Zona { get; set; }
        public string UrlServicio { get; set; }
        public int TipoNivelCliente { get; set; }
        public string NombreCliente { get; set; }

        public DateTimeOffset Fecha { get; set; }

    }
}
