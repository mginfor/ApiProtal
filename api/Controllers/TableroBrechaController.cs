using Microsoft.AspNetCore.Mvc;
using Contracts;
using Entities.EPModels;
using System.Collections.Generic;
using api.Helpers;
using System.Security.Claims;
using static iText.StyledXmlParser.Css.Parse.CssDeclarationValueTokenizer;
using Microsoft.AspNetCore.Identity;
using System;

namespace api.Controllers
{

    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TableroBrechaController : ControllerBase 
    {
        private ITablero_brecha _tablerobrechaservice;
        private IUsuarioPortalService _usuarioPortalService;

        public TableroBrechaController(ITablero_brecha tablero_Brecha, IUsuarioPortalService usuarioPortalService)
        {
            _tablerobrechaservice = tablero_Brecha;
            _usuarioPortalService = usuarioPortalService;
      
        }


        [Route("[action]/{idcliente}")]
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
