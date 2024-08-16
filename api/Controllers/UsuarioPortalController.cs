using api.Helpers;
using Contracts;
using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
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
            IClienteService clienteService

            )
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _configuration = configuration;
            _logService = logService;
            _servicioVinculadoService = servicioVinculadoService;
            _clienteService = clienteService;

            //entidad cliente
        }



        // GET: api/<UsuarioController>
        //[Authorize]
        [HttpGet]
        public IActionResult GetUsuario()
        {
            var usuario = _usuarioService.GetAllUsuario();
            return Ok(usuario);
        }

        //[Authorize]
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var salida = new GenericResponse();
            salida.data = _usuarioService.GetById(id);
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
                    mail.EnviarCorreoGraph(user.correo, user.nombreUsuario + " " + user.apellidoUsuario, user.Pass);
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


        [HttpPost("loginAsync")]
        public async Task<IActionResult> loginAsync(AuthenticateRequestPortal model)
        {
            var salida = new GenericResponse();
            var dto = await _usuarioService.LoginAsync(model);

            if (!string.IsNullOrEmpty(dto.RefreshToken))
            {
                SetRefreshTokenInCookie(dto.RefreshToken);
            }


            if (dto.EstaAutenticado)
            {
                salida.data = dto;
                salida.status = true;
                return Ok(salida);
            }

            salida.data = new { message = dto.Mensaje };
            salida.status = false;
            return BadRequest(salida);

         
        }


        // Endpoint para validar un token
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken(string token)
        {
            var result = await _usuarioService.ValidateTokenAsync(token);

            if (!string.IsNullOrEmpty(result.RefreshToken))
            {
                SetRefreshTokenInCookie(result.RefreshToken);
            }
            if (result.EstaAutenticado)
            {

                return Ok(result);
            }
            return BadRequest(result);

        }

        // Endpoint para refrescar un token
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response = await _usuarioService.Refreshtoken(refreshToken);
            if(!string.IsNullOrEmpty(response.RefreshToken))
                SetRefreshTokenInCookie(response.RefreshToken);
            return Ok(response);
        }

        private void SetRefreshTokenInCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(10)
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }


        [HttpPost("login")]
        public IActionResult login(AuthenticateRequestPortal model)
        {
            var salida = new GenericResponse();
            var dto = _usuarioService.Authenticate(model);


            if (dto.usuario == null)
            {
                salida.data = new { message = dto.Mensaje };
                salida.status = false;
                return BadRequest(salida);
            }


            var serviciosVinculados = _servicioVinculadoService.getServicioPorIdCliente(dto.usuario.idCliente);

            //Información cliente
            var informacionCliente = _clienteService.getClienteByidCliente(dto.usuario.idCliente);


            if (serviciosVinculados != null)
            {
                dto.usuario.UrlServicio = serviciosVinculados.urlServicio;


            }

            if (informacionCliente != null)
            {
                //info cliente
                dto.usuario.tipoNivelCliente = informacionCliente.nivel;
            }


            salida.data = dto;

            return Ok(salida);
        }



        [HttpGet("ValidarToken")]
        public IActionResult GetToken([FromQuery] string token)
        {
            var salida = new GenericResponse();

            if (string.IsNullOrWhiteSpace(token))
            {
                salida.data = new { message = "Token is required" };
                salida.status = false;
                return BadRequest(salida);

            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Key"]);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _configuration["JWT:Issuer"],
                    ValidAudience = _configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken)
                {
                    // Optionally, you can check additional claims here
                    var userId = principal.Claims.FirstOrDefault(c => c.Type == "uid")?.Value;
                    if (userId != null)
                    {
                        // Optionally fetch user by id or other claims
                        // var user = _usuarioService.GetUserById(int.Parse(userId));
                        // salida.data = user;
                        salida.data = new { message = "Token is valid" };
                        salida.status = true;
                        return Ok(salida);
                    }
                }

                salida.data = new { message = "Token is invalid" };
                salida.status = false;
                return Unauthorized(salida);



            }
            catch (Exception ex)
            {
                salida.data = new { message = "Token validation failed", details = ex.Message };
                salida.status = false;
                return Unauthorized(salida);

            }
        }






        [Authorize]
        [HttpGet("Validarlogin")]
        public IActionResult GetLogin(string token)
        {
            var salida = new GenericResponse();
            var user = _usuarioService.GetUserByToken(token);

            if (user == null)
            {
                salida.data = new { message = "Token incorrecto" };
                salida.status = false;
                return BadRequest(salida);
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
