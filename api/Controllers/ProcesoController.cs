using api.Helpers;
using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
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
        IUsuarioPortalService _usuarioPortalService; 
        public ProcesoController(IProcesoService conexion, IUsuarioPortalService usuarioPortalService)
        {
            _procesoService = conexion;
            _usuarioPortalService = usuarioPortalService;
        }



        [HttpGet]
        public IActionResult Get()
        {
            return Ok(null);
        }

        [HttpGet("{proceso}")]
        public IActionResult Get(string proceso)
        {
             
            var condicion = JsonSerializer.Deserialize<ProcesoEP>(proceso);
            var procesos = _procesoService.getProcesosByProcesoEP(condicion);
            return Ok(procesos);
        }


        [HttpGet("[action]")]
        [ProducesResponseType(200, Type = typeof(ProcesoReportabilidadDTO))]
        public IActionResult GetProcesosReportabilidad()
        {
            var procesos = _procesoService.GetProcesosReportabilidad();
            //fake results
            var procesosDto = new List<ProcesoReportabilidadDTO>();

            foreach (var item in procesos)
            {
                procesosDto.Add(new ProcesoReportabilidadDTO
                {
                    ResultadoChilevalora = item.COD_RESULTADO == 1 ? "Competente" : "No Competente",
                    Perfil = item.DESC_PERFIL,
                    Rut = item.RUN_CANDIDATO.ToString("N0") + "- " + item.DIG_CANDIDATO,
                    RutEvaluador = item.RUN_EVALUADOR + "- " + item.DIG_EVALUADOR,
                    Auditoria = item.FECHA_AUDITORIA,
                    Elegibilidad = item.FECHA_ELEGIBILIDAD,
                    FECHAINICIO = item.FECHA_INICIO,
                    CuestionarioJefaturaDirecta = item.FECHA_REAL_EJD,
                    PruebaTeorica = item.FECHA_REAL_PCT,
                    EvaluacionPractica1 = item.FECHA_REAL_PROT,
                    EvaluacionPractica2 = item.FECHA_REAL_PROT2,
                    Socializacion = item.FECHA_SOCIALIZACION,
                    FECHATERMINO = item.FECHA_TERMINO,
                    ProcesoValidacionChilevalora = item.FECHA_VALIDACION_CHILE_VALORA,
                    NombreTrabajador = item.NOMBRE_CANDIDATO,
                    Empresa = item.NOMBRE_CLIENTE,
                    Evaluador = item.NOMBRE_EVALUADOR,
                    Faena = item.NOMBRE_FAENA,
                    ProcentajeAvance = item.PORCENTAJE_AVANCE,
                    EvidenciaDocumental = item.FECHA_EI,
                    EsElegible = item.FLG_ELEGIBILIDAD
                });
            }

            //mapeo dto


            return Ok(procesosDto);
        }
        private int GetIdUser()
        {
            var user = HttpContext.Items["User"] as UsuarioPortal;
            return Convert.ToInt32(user.id);

        }
        private UsuarioPortal GetUser()
        {
            var user = HttpContext.Items["User"] as UsuarioPortal;

            return user;

        }

    }
}
