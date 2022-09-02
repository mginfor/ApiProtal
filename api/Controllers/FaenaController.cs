using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Entities.DbModels;
using Services;

namespace api.Controllers
{
    public class FaenaController : ControllerBase
    {
        private IFaenaService _faenaService;
        private IEvaluacionExportarService _evaluacionService;
        
        public FaenaController(IFaenaService faenaService,IEvaluacionExportarService evaluacionService)
        {
            _faenaService = faenaService;
            _evaluacionService = evaluacionService;
        }

        [Route("[action]")]
        [HttpGet]
        public IActionResult getAllFaenas()
        {
            var faenas = _faenaService.getAllFaenas();
            var faenasEp = new List<FaenaEP>();
            foreach (var faena in faenas)
            {
                faenasEp.Add(new FaenaEP(faena.id, faena.descripcion, faena.vigencia, faena.zona));

            }
            return Ok(faenasEp);
        }

        [Route("[action]/{idCliente}/{idPerfil}")]
        [HttpGet]
        public IActionResult getAllFaenasByCliente(int idCliente, int idPerfil)
        {
            var faenas = _faenaService.getAllFaenas();
            var faenasEp = new List<FaenaEP>();
            var evals = _evaluacionService.getAllEvaluationsByClienteByPerfil(idCliente, idPerfil)
                .GroupBy(evaluacion => evaluacion.idFaena)
                .Select(x => x.FirstOrDefault()).ToList();
            foreach (var eval in evals)
            {
                var faenaAux = faenas.Find(faena => faena.id == eval.idFaena);
                if (faenaAux != null) faenasEp.Add(new FaenaEP(faenaAux.id, faenaAux.descripcion, faenaAux.vigencia, faenaAux.zona));
            }
            return Ok(faenasEp.OrderBy(x => x.descripcion));
        }
    }
    
}
