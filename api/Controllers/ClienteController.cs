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
using Microsoft.Extensions.Configuration;

namespace api.Controllers
{

    public class ClienteController : ControllerBase
    {
        private IClienteService _clienteService;        
        

        public ClienteController(IClienteService clienteService)
        {
            _clienteService = clienteService;
           
        }


        [Route("[action]/{idCliente}")]
        [HttpGet]
        public IActionResult getClienteServicio(int idCliente)
        {
            var cliente = _clienteService.getClienteByidCliente(idCliente);
            

            return Ok(cliente);
        }



        }
}
