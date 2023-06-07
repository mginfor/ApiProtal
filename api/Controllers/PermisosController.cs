using Contracts.Generic;
using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Entities.EPModels;
using api.Helpers;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {

        private IPermisosServices _permisosService;



        public PermisosController(
            IPermisosServices permisosServices
            )
        {
            
            _permisosService = permisosServices;
            //entidad cliente
        }

        [HttpGet]
        public IActionResult GetPermiso()
        {
            var permisos = _permisosService.GetAllPermisos();
            return Ok(permisos);
        }

        [HttpGet("{id}")]
        public IActionResult GetPermisoById(int id)
        {
            var salida = new GenericResponse();
            salida.data = _permisosService.GetPermisosByid(id);
            return Ok(salida);
        }




    }
}
