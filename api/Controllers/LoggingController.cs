using api.Helpers;
using Contracts;
using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private IUsuarioPortalService _usuarioService;
        private readonly IConfiguration _configuration;
        private ILogTableroService _logTablero;
        private IServicioVinculadoService _servicioVinculadoService;
        private IClienteService _clienteService;

        public LoggingController(ILoggerManager logger,
                       IUsuarioPortalService usuarioService,
                                  IConfiguration configuration,
                                             ILogTableroService logService,
                                                        IServicioVinculadoService servicioVinculadoService,
                                                                   IClienteService clienteService
                       )
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _configuration = configuration;
            _logTablero = logService;
            _servicioVinculadoService = servicioVinculadoService;
            _clienteService = clienteService;

            
        }


        [HttpPost("LogEvent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LogEvent([FromBody] LogTableroResponse logModel)
        {

            if (logModel == null)
            {
                return BadRequest("LogModel object is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid model object");
            }
            try
            {
                await _logTablero.LogEvent(logModel);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }








    }
}
