using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Entities.DbModels;

namespace api.Controllers
{
    public class SectorController : ControllerBase
    {
        private ISectorService _sectorService;
        public SectorController(ISectorService sectorService)
        {
            _sectorService = sectorService;
        }
        [Route("[action]")]
        [HttpGet]
        public IActionResult getAllSectores()
        {
            var sectores = _sectorService.getAllSectores();
            return Ok(sectores);
        }
    }
}
