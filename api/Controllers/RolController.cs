using api.Helpers;
using Contracts;
using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services;

namespace api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RolController : ControllerBase
    {

        private IRolService _rolService;

        public RolController(
            IRolService rolService)
        {
            
            _rolService = rolService;

            //entidad cliente
        }

        [HttpGet]
        public IActionResult GetPermiso()
        {
            var permisos = _rolService.GetAllRol();
            return Ok(permisos);
        }

        [HttpGet("{id}")]
        public IActionResult GetPermisoById(int id)
        {
            var salida = new GenericResponse();
            salida.data = _rolService.GetRolByid(id);
            return Ok(salida);
        }



        //[HttpPut("{id}")]
        //public IActionResult UpdateRol(int id, Rol rol)
        //{
        //    var existingRol = _rolService.GetRolByid(id);
        //    if (existingRol == null)
        //    {
        //        return NotFound();
        //    }

        //    existingRol.NombreRol = rol.NombreRol;
        //    // Actualiza otras propiedades del rol según sea necesario

        //    _rolService.UpdateRol(existingRol);
        //    return Ok();
        //}

        //[HttpDelete("{id}")]
        //public IActionResult DeleteRol(int id)
        //{
        //    var existingRol = _rolService.GetRolByid(id);
        //    if (existingRol == null)
        //    {
        //        return NotFound();
        //    }

        //    _rolService.DeleteRol(id);
        //    return Ok();
        //}





    }
}
