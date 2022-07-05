using Entities.DbModels;

namespace Entities.EPModels
{
    public class AuthenticateResponsePortal
    {
        public UsuarioPortalEP usuario { get; set; }
        public string Token { get; set; }
        public int Expira { get; set; }


        public AuthenticateResponsePortal(UsuarioPortal user, string token)
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
             

            };
            Token = token;
            Expira = 8;
        }
    }
}
