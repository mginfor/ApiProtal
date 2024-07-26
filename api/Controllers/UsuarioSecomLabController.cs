using api.Helpers;
using Contracts;
using Contracts.Generic;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioSecomLabController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private IUsuarioSecomLabService _usuarioService;

        public UsuarioSecomLabController(ILoggerManager logger,
            IUsuarioSecomLabService usuarioService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequestSecomlab model)
        {
            var salida = new GenericResponse();
            var response = _usuarioService.Authenticate(model);
            if (response == null)
            {
                salida.data = new { message = "Usuario o Contraseña incorrectos" };
                salida.status = false;
                return BadRequest(salida);
            }

            salida.data = response;
            return Ok(salida);
        }


        [HttpGet]
        public IActionResult Get()
        {
            var salida = new GenericResponse();
            salida.data = _usuarioService.findAll("nivelUsuario").ToList();

            return Ok(salida);
        }

        // GET api/<UsuarioController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<UsuarioController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UsuarioController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsuarioController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
