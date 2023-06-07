using api.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;

namespace api.Controllers
{
    public class BaseController : ControllerBase
    {

        /// <summary>
        /// Función que retorna el id del usuario a través del token.
        /// IMPORTANTE: Para usar este método debe tener la etiqueta de autorización
        /// </summary>
        /// <returns>idUsuario</returns>
        public int GetIdUser()
        {
            AutorizacionHelper helper = new();
            var idUsuarioString = User.Identity.Name;
            return Convert.ToInt32(idUsuarioString);

        }
    }

}




