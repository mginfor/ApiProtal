using api.Helpers;
using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace api.Controllers
{
    //[Authorize]
    [Route("[controller]/[action]")]
    [ApiController]
    public class tratamientoController : ControllerBase
    {
        private IProcesoService _procesoService;
        public tratamientoController(IProcesoService conexion)
        {
            _procesoService = conexion;
        }


        [HttpPost()]
        public IActionResult GetTratamiento(ProcesoEP proceso)
        {
            var tratamientos = _procesoService.getProcesosByProcesoTratamiento(proceso);
            return Ok(tratamientos);
        }


    }
}










