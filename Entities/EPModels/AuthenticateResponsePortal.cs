using Entities.DbModels;
using System.Collections.Generic;

namespace Entities.EPModels
{
    public class AuthenticateResponsePortal
    {
        public UsuarioPortalEP usuario { get; set; }
        public string Token { get; set; }
        public int Expira { get; set; }


        public AuthenticateResponsePortal(UsuarioPortal user, string token,List<Permisos> permisos)
        {
            usuario = new UsuarioPortalEP()
            {
                id = user.id,
                nombreUsuario = user.nombreUsuario + " " + user.apellidoUsuario,
                correo = user.correo,
                idCliente = user.idCliente,
                nombrecliente = user.cliente.nombre,
                zona = user.zona,
                cargo = user.cargo,
                UrlServicio = "",
                rol = user.rol,
                permisos = permisos
            };
            Token = token;
            Expira = 8;
        }
    }
}
