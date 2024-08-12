using api.Helpers;
using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{

    [Route("[controller]/[action]")]
    [ApiController]
    public class tratamientoController : ControllerBase
    {
        private IProcesoService _procesoService;
        public tratamientoController(IProcesoService conexion)
        {
            _procesoService = conexion;
        }

        [Authorize(Roles = EnumRol.Admin + "," + EnumRol.Tratamiento + "," + EnumRol.DescargaTratamiento)]
        [HttpPost()]
        public IActionResult GetTratamiento(ProcesoEP proceso)
        {
            var tratamientos = _procesoService.getProcesosByProcesoTratamiento(proceso);
            return Ok(tratamientos);
        }


    }

}

