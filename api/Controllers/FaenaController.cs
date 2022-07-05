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
                faenasEp.Add(new FaenaEP
                {
                    id = faena.id,
                    descripcion = faena.descripcion,
                    vigencia = faena.vigencia,
                    zona = faena.zona
                });
            }
            return Ok(faenasEp);
        }
        
        [Route("[action]/{idCliente}")]
        [HttpGet]
        public IActionResult getAllFaenasByCliente(int idCliente)
        {
            var faenas = _faenaService.getAllFaenas();
            var faenasEp = new List<FaenaEP>();
            var evals = _evaluacionService.getAllEvaluationsByCliente(idCliente);
            foreach (var eval in evals)
            {
                foreach (var faena in faenas)
                {
                    if (eval.idFaena == faena.id)
                    {
                        faenasEp.Add(new FaenaEP
                        {
                            id = faena.id,
                            descripcion = faena.descripcion,
                            vigencia = faena.vigencia,
                            zona = faena.zona
                        });
                    }
                }
            }
            return Ok(faenasEp.GroupBy(x => x.id).Select(x => x.FirstOrDefault()).OrderBy(x => x.descripcion));
        }
    }
    
}
