using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class DatosUsuarios
    {
        public string Mensaje {  get; set; }
        public bool EstaAutenticado { get; set; }
        public string Correo { get; set; }
        public Rol Rol {  get; set; }
        public string Token {  get; set; }
        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }

        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public int IdCliente { get; set; }

        public string UrlServicio { get; set; }
        public string TipoNivelCliente { get; set; }

        public List<Permisos> Permisos { get; set; }


    }
}
