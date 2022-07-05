using Entities.EPModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using api.Helpers;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MailController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = new { 
                Subject= "Asunto del Mensaje",
                Messaje= "Cuerpo del mensaje (HTML)",
			    To= "[correo1,correo2,correoN]",
                CC= "[correo1,correo2,correoN]",
                Cco = "[correo1,correo2,correoN]",
                Attachment=new
                {
                    AttName="Nombre Archivo",
                    Att="string Base64"
                }
            };
            return Ok(result);
        }

        [HttpPost]
        public ActionResult<IEnumerable<bool>> post([FromBody] MailMap mensaje)
        {

            MailHelper mail = new MailHelper(_configuration);
            var resultado = mail.sendMail(mensaje);

            return Ok(resultado);
        }

        
    }
}
