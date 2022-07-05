using Microsoft.AspNetCore.Mvc;
using Contracts;
using Entities.EPModels;
using System.Collections.Generic;

namespace api.Controllers
{
    public class TableroBrechaController : ControllerBase
    {
        private ITablero_brecha _tablerobrechaservice;
        public TableroBrechaController(ITablero_brecha tablero_Brecha)
        {
            _tablerobrechaservice = tablero_Brecha;
        }


        [Route("[controller]/[action]/{idcliente}")]
        [HttpGet]
        public IActionResult GetCantidadTableroBrecha( int idcliente)
        {
            try
            {
                var listado = _tablerobrechaservice.getCantidadTableroBrecha(idcliente);
                var listadoEP = new List<Tablero_Brecha>();

                foreach (var item in listado)
                {
                    listadoEP.Add(new Tablero_Brecha { Brecha = item.brecha, Operador = item.q_op });
                }

                 return Ok(listadoEP);

            }
            catch (System.Exception ex)
            {

                return  BadRequest(ex);
            }
           
        }

    }
}
