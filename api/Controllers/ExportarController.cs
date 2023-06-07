 using api.Helpers;
using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;


namespace api.Controllers
{
    public class ExportarController : ControllerBase
    {
        private IExportarService _exportarService;
        private IUsuarioPortalService _usuarioPortalService;
        public ExportarController(IExportarService exportarService, IUsuarioPortalService usuarioPortalService)
        {
            _exportarService = exportarService;
            _usuarioPortalService = usuarioPortalService;
        }

        [Route("[action]/{idPerfil}/{idFaena}/{idCliente}")]
        [HttpGet]
        public IActionResult getDatosExportacion(int idPerfil, int idFaena, int idCliente)
        {
            var salida = new GenericResponse();
            var resultado = _exportarService.getDatosExportacion(idPerfil, idFaena, idCliente).ToList();

            if (resultado == null)
            {
                salida.status = false;
                salida.data = new { message = "Sin resultados" };
                return BadRequest(salida);
            }
            else
            {
                salida.data = resultado;
                return Ok(salida);
            }

        }

        [Route("[action]/{idEvaluacion}")]
        [HttpGet]
        public IActionResult getProcesoBrecha(int idEvaluacion)
        {
            var salida = new GenericResponse();
            var resultado = _exportarService.getProcesoBrecha(idEvaluacion).ToList();

            if (resultado == null)
            {
                salida.status = false;
                salida.data = new { message = "Sin resultados" };
                return BadRequest(salida);
            }
            else
            {
                salida.data = resultado;
                return Ok(salida);
            }

        }


        [Route("[action]")]
        [HttpPost]

        public IActionResult getExcelBrechaCandidatos([FromBody] ExcelBrechaCandidatos excelBrechaCandidatos)
        {
          
            //AutorizacionHelper autorizacionHelper = new AutorizacionHelper();

            //bool isAuthorized = autorizacionHelper.EstaAutorizado(excelBrechaCandidatos.idUsuario, AutorizacionHelper.Roles.Descarga);

            //if (!isAuthorized)
            //{
               
            //    return StatusCode(403, "No tiene permiso.");
            //}

            excelBrechaCandidatos.idCliente = _usuarioPortalService.GetById(excelBrechaCandidatos.idUsuario).idCliente;
            var libro = _exportarService.GenerarExcelBrechasCandidatos(excelBrechaCandidatos);
            AdjuntoResponse documento;
            using (var memo = new MemoryStream())
            {
                libro.SaveAs(memo);
                var nombreExcel = string.Concat("Reporte", DateTime.Now.ToString(), ".xlsx");
                documento = new AdjuntoResponse(nombreExcel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", Convert.ToBase64String(memo.ToArray()));
            }

            return Ok(new GenericResponse(true, documento, "Documento generado exitosamente"));
        }
    }
}

