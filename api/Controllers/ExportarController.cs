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
        public ExportarController(IExportarService exportarService)
        {
            _exportarService = exportarService;
        }

        [Route("[action]/{idPerfil}/{idFaena}/{idCliente}")]
        [HttpGet]
        public IActionResult getDatosExportacion(int idPerfil, int idFaena, int idCliente)
        {
            var salida = new GenericResponse();
            var resultado = _exportarService.getDatosExportacion(idPerfil,idFaena, idCliente).ToList();

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
        public IActionResult getExcelBrechaCandidatos([FromBody]ExcelBrechaCandidatos excelBrechaCandidatos)
        {
            var libro = _exportarService.GenerarExcelBrechasCandidatos(excelBrechaCandidatos);

            using (var memo = new MemoryStream())
            {

                libro.SaveAs(memo);
                var nombreExcel = string.Concat("Reporte tickets", DateTime.Now.ToString(), ".xlsx");
                var archivos = File(memo.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", nombreExcel);
                return archivos;
            }
        }

















    }

}
