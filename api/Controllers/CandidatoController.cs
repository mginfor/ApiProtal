using Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Entities.DbModels;
using Entities.EPModels;
using Services;
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{
    public class CandidatoController : ControllerBase
    {
        private ICandidatoService _candidatoService;

        public CandidatoController(ICandidatoService candidatoService)
        {
            _candidatoService = candidatoService;

        }

        [Route("[action]")]
        [HttpPost]
        public ActionResult<PassRequest> getCandidato([FromBody]PassRequest passRequest)
        {

            var candidato = _candidatoService.getCandidato(passRequest.idCandidato, passRequest.passBrecha);

            if (candidato is null)
            {

                return BadRequest(new GenericResponse(false, null, "contraseña incorrecta"));

            }

            return Ok(new GenericResponse(true, candidato, "contraseña Correcta :D"));

            //return Ok(candidato);

        }

    }
}
