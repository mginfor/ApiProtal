using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CriterioController : ControllerBase
    {
        private readonly ICriterioService _criterioService;

        public CriterioController(ICriterioService criterioService)
        {
            _criterioService = criterioService;
        }


        [HttpGet("{tgesEvalPctId}")]
        public IActionResult GetCriteriosByTgesEvalPct(int tgesEvalPctId)
        {
            var criterios = _criterioService.GetCriteriosByTgesEvalPct(tgesEvalPctId);

            if (criterios == null)
            {
                return NotFound();
            }

            return Ok(criterios);
        }
    }
}
