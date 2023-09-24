﻿using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using Entities.DbModels;
using Services;
using api.Helpers;

namespace api.Controllers
{
    public class PerfilController : ControllerBase
    {
        private IPerfilService _perfilService;
        private ISectorService _sectorService;
        private IEvaluacionExportarService _evaluacionService;

        public PerfilController(IPerfilService perfilService,
            ISectorService sectorService, IEvaluacionExportarService evaluacionService)
        {
            _perfilService = perfilService;
            _sectorService = sectorService;
            _evaluacionService = evaluacionService;
        }
        [Route("[action]")]
        [HttpGet]
        public IActionResult getAllPerfiles()
        {
            var perfiles = _perfilService.getAllPerfiles();
            var perfilesEp = new List<PerfilEP>();
            var sectores = _sectorService.getAllSectores();
            foreach (var perfil in perfiles)
            {
                if (perfil.flgVigencia != 0)
                {
                    foreach (Sector item in sectores)
                    {
                        if (item.vigencia != 0)
                        {
                            if (item.id == (int)perfil.codigoSector)
                            {
                                perfilesEp.Add(new PerfilEP
                                {
                                    id = perfil.id,
                                    codigoPerfil = perfil.codigoPerfil,
                                    descripcionPerfil = perfil.descripcionPerfil,
                                    codigoSector = perfil.codigoSector,
                                    codigoSubSector = perfil.codigoSubSector
                                });
                            }
                        }
                    }
                }

            }
            return Ok(perfilesEp);
        }

        [Route("[action]/{idCliente}")]
        [HttpGet]
        public IActionResult getAllPerfilesByCliente(int idCliente)
        {
            var perfiles = _perfilService.getAllPerfiles();
            var perfilesEp = new List<PerfilEP>();
            var sectores = _sectorService.getAllSectores();
            var evals = _evaluacionService.getAllEvaluationsByCliente(idCliente);
            var idPerfilesConEvaluacion = evals.DistinctBy(x =>x.idPerfil).Select(x => x.idPerfil).ToList();
            var perfilesFiltrados = perfiles.Where(x => idPerfilesConEvaluacion.Any(y => y == x.id));
            var sectoresFiltrados = sectores.Where(x => perfilesFiltrados.Any(y => (int)y.codigoSector == x.id));

            var request = perfilesFiltrados.Select(x => new PerfilEP
            {
                id = x.id,
                codigoPerfil = x.codigoPerfil,
                descripcionPerfil = x.descripcionPerfil,
                codigoSector = x.codigoSector,
                codigoSubSector = x.codigoSubSector
            });
            perfilesEp.AddRange(request);

            
            return Ok(perfilesEp.GroupBy(x => x.id).Select(x => x.FirstOrDefault()).OrderBy(x => x.descripcionPerfil));
        }
    }
}
