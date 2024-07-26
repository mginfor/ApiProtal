using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using api.Helpers;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace api.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class GenericController : ControllerBase
    {

        private IEstadoProcesoService _estadoProcesoService;
        private IProcesoService _procesoService;
        private IClienteService _clienteService;

        public GenericController(IEstadoProcesoService estadoProceso,
            IProcesoService procesoService, IClienteService clienteService)
        {
            _estadoProcesoService = estadoProceso;
            _procesoService = procesoService;
            _clienteService = clienteService;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult GetEstadosProcesos()
        {
            var estados = _estadoProcesoService.GetAllEstadoProcesos();
            return Ok(estados);
        }

        [Route("[action]/{folio}")]
        [HttpGet]
        [Authorize(Policy = "GestionInformes")]
        public IActionResult getDataInformeByFolio(int folio)
        {
            var salida = new GenericResponse();
            var resultado = _procesoService.getProcesosByIdInforme(folio).FirstOrDefault();
            if (folio > 0)
            {
                if (resultado == null)
                {
                    salida.status = false;
                    salida.data = new { message = "Folio no Encontrado" };
                    return BadRequest(salida);
                }
                else
                {
                    salida.data = resultado;
                    return Ok(salida);
                }

            }
            else
            {
                salida.status = false;
                salida.data = new { message = "No Folio" };
                return BadRequest(salida);
            }
        }

        [Route("[action]/{hash}")]
        [HttpGet]
        public IActionResult getDataInformeByHash(string hash)
        {
            var salida = new GenericResponse();
            var results = _procesoService.getProcesosByHash(hash).ToList();
            List<HistoricoProcesoEP> data = new();

            if (!string.IsNullOrEmpty(hash))
            {
                if (results.Count == 0)
                {
                    salida.status = false;
                    salida.data = new { message = "Hash inválido" };
                    return BadRequest(salida);
                }
                else
                {
                    string nombreCandidato = "";
                    string run = "";
                    string dniPasaporte = "";
                    int idCandidato = 0;
                    foreach (var item in results)
                    {

                        data.Add(new(item.idEvaluacion, item.nombreCliente, item.perfil, item.fechaVigenciaInforme != null ? item.fechaVigenciaInforme.Value.ToShortDateString() : null, item.modelo, item.descResultado, item.nombreFaena));
                        nombreCandidato = item.nombreCandidato;
                        run = item.runCandidato;
                        idCandidato = item.idCandidato;
                    }

                    salida.data = (new HistorialCandidatoEP(nombreCandidato, run, dniPasaporte , data, idCandidato));


                    return Ok(salida);
                }

            }
            else
            {
                salida.status = false;
                salida.data = new { message = "Hash requerido" };
                return BadRequest(salida);
            }





        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult getAllCliente()
        {
            var clientes = _clienteService.getAllCliente();
            var clientesEp = new List<ClienteEP>();
            foreach (var cliente in clientes)
            {
                clientesEp.Add(new ClienteEP
                {
                    idCliente = cliente.id,
                    rut = cliente.run.ToString() + cliente.digito,
                    nombreCliente = cliente.nombre,
                    nombreFantasia = cliente.nombreFantasia,
                    tipoNivelCliente = cliente.nivel
                });
            }
            return Ok(clientesEp.OrderBy(X => X.nombreCliente));
        }


        [Route("[action]/{idEvaluacion}")]
        [HttpGet]
        public IActionResult getbrechasByHash(int idEvaluacion)
        {
            var salida = new GenericResponse();
            var resultado = _procesoService.getbrechasByHash(idEvaluacion).ToList();

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




        [Route("[action]/{folio}/{run}/{cliente}")]
        [HttpGet]
        public IActionResult getDataInformeByFolio(string folio, string run, int cliente)
        {
            var salida = new GenericResponse();

            try
            {
                var resultado = _procesoService.getProcesosByIdInformeNuevo(folio, run, cliente).FirstOrDefault();

                if (resultado == null)
                {
                    salida.status = false;
                    salida.data = new { message = "Folio no Encontrado" };
                    return BadRequest(salida);
                }
                else
                {
                    salida.data = resultado;
                    return Ok(salida);
                }
            }
            catch (ArgumentException ex)
            {
                salida.status = false;
                salida.data = new { message = ex.Message };
                return BadRequest(salida);
            }
        }



    }
}