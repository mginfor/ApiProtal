using Microsoft.AspNetCore.Mvc;
using Contracts;
using System.Collections.Generic;
using Entities.EPModels;
namespace api.Controllers
{
    public class GraficoBrechaController : ControllerBase
    {
        private IGraficoBrechaService _graficoBrechaService;
        public GraficoBrechaController(IGraficoBrechaService graficoBrechaService)
        {
            _graficoBrechaService = graficoBrechaService;
        }
        [Route("[controller]/[action]/{idCliente}")]
        [HttpGet]
        public IActionResult getCantidadOperadoresBrechasPerfil(int idCliente)
        {
            try
            {
                var listado = _graficoBrechaService.getCantidadOperadoresBrechasPerfil(idCliente);
                var listadoEp = new List<Grafico_Brecha>();

                foreach (var item in listado)
                {
                    listadoEp.Add(new Grafico_Brecha
                    {
                        perfil = item.perfil,
                        operadores_brechas = item.operadores_brechas
                        
                    });
                }


                return Ok(listadoEp);

            }
            catch (System.Exception)
            {
                return BadRequest();
            }
        }
    }
}

