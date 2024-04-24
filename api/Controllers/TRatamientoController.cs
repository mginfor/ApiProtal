using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;

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
}
