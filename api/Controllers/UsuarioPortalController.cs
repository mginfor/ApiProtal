using api.Helpers;
using Contracts;
using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioPortalController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private IUsuarioPortalService _usuarioService;
        private readonly IConfiguration _configuration;
        private ILogService _logService;
        private IServicioVinculadoService _servicioVinculadoService;
        private IClienteService _clienteService;


        
        public UsuarioPortalController(ILoggerManager logger,
            IUsuarioPortalService usuarioService,
            IConfiguration configuration,
            ILogService logService,
            IServicioVinculadoService servicioVinculadoService, 
            IClienteService clienteService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _configuration = configuration;
            _logService = logService;
            _servicioVinculadoService = servicioVinculadoService;
            _clienteService = clienteService; //entidad cliente
        }



        // GET: api/<UsuarioController>
        //[Authorize]
        [HttpGet]
        [HttpGet]
        public IActionResult GetAllUsuarios()
        {
            try
            {
                var usuarios = _usuarioService.GetAllUsuarios(); 
                if (usuarios == null || !usuarios.Any())
                {
                    return NotFound("No se encontraron usuarios.");
                }

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
               
                return StatusCode(500, "Internal server error");
            }
        }

        //[Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var usuario = _usuarioService.GetById(id);

            // Comprobar si el usuario no fue encontrado
            if (usuario == null)
            {
                return NotFound(new GenericResponse
                {
                    status = false,
                    message = $"No se encontró el usuario con el ID {id}.",
                    data = null
                });
            }

            // Si se encuentra el usuario, devolver los datos
            var salida = new GenericResponse
            {
                status = true,
                data = usuario,
                message = "Usuario encontrado con éxito."
            };
            return Ok(salida);
        }

        [HttpPost("preLogin")]
        public IActionResult preLogin(AuthenticateRequestPortal model)
        {
            var salida = new GenericResponse();
            var user = _usuarioService.PreLogin(model);

            try
            {

                if (user == null)
                {
                    salida.data = new { message = "Usuario inválido" };
                    salida.status = false;
                    return Unauthorized(salida);
                }

                if (user.activo != 0)
                {
                    //enviarCodigoPorCorreo(user.correo, user.nombreUsuario + " " + user.apellidoUsuario, user.clave);
                    MailHelper mail = new MailHelper(_configuration);
                    mail.EnviarCorreoGraph(user.correo, user.nombreUsuario + " " + user.apellidoUsuario, user.clave);
                    var aux = user.correo.Split("@");
                    salida.data = new { message = "correo enviado a " + aux[0].Substring(0, 3) + "xxxxx@" + aux[1] };
                    return Ok(salida);

                }
                else
                {

                    salida.data = new { message = "Cuenta no activa" };
                    salida.status = false;
                    return StatusCode(StatusCodes.Status403Forbidden, salida);

                }

            }
            catch (Exception ex)
            {

                return Unauthorized(ex);
            }

           



        }


        [HttpPost("login")]
        public IActionResult login(AuthenticateRequestPortal model)
        {
            var salida = new GenericResponse();
            var user = _usuarioService.Authenticate(model);

            if (user == null)
            {
                salida.data = new { message = "Usuario o Contraseña incorrectos" };
                salida.status = false;
                return BadRequest(salida);
            }

            var serviciosVinculados = _servicioVinculadoService.getServicioPorIdCliente(user.usuario.idCliente);

            //Información cliente
            var informacionCliente = _clienteService.getClienteByidCliente(user.usuario.idCliente);


            _logService.create(new Log() { idUsuario = user.usuario.id, fechaIngreso = DateTime.Now, clave = model.Codigo.ToString() });


            if (serviciosVinculados != null)
            {
                user.usuario.UrlServicio = serviciosVinculados.urlServicio;

              
            }

            if (informacionCliente != null)
            {
                //info cliente
                user.usuario.tipoNivelCliente = informacionCliente.nivel;
            }


            salida.data = user;

            return Ok(salida);
        }

        #region Metodos

        #endregion



        private void enviarCodigoPorCorreo(string destinatario, string usuario, int codigo)
        {

            MailHelper mail = new MailHelper(_configuration);
            MailMap mailMap = new MailMap();
            string relativePath = @"Plantillas\mensaje.html";
            var ruta = Path.GetFullPath(relativePath);
            string html = System.IO.File.ReadAllText(ruta);

            mailMap.Messaje = string.Format(html, usuario, codigo.ToString());
            mailMap.Subject = "codigo de verificación: " + codigo.ToString();
            mailMap.To = new List<string>(new string[] { destinatario });

            mail.sendMail(mailMap);
        }

     
    }
}
