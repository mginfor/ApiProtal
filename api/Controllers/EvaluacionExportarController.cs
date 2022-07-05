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
    public class EvaluacionExportarController : ControllerBase
    {
        private IEvaluacionExportarService _evaluacionService;
        public EvaluacionExportarController(IEvaluacionExportarService evaluacionService)
        {
            _evaluacionService = evaluacionService;
        }
        
    }
}
