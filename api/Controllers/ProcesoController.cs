using api.Helpers;
using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ProcesoController : ControllerBase
    {
        private IProcesoService _procesoService;
        public ProcesoController(IProcesoService conexion)
        {
            _procesoService = conexion;
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok(null);
        }


        [HttpGet("{proceso}")]
        public IActionResult Get(string proceso)
        {
            var condicion= JsonSerializer.Deserialize<ProcesoEP>(proceso);
            var procesos = _procesoService.getProcesosByProcesoEP(condicion);
            return Ok(procesos);
        }

        


    }
}
