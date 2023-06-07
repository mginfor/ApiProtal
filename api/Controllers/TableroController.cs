using api.Helpers;
using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;


namespace api.Controllers
{

    
    [Authorize]
    public class TableroController : BaseController
    {
        private ITableroService _conexion;
        public TableroController(ITableroService conexion)
        {
            _conexion = conexion;
        }

        // GET: api/<TableroController>
        [HttpGet]
        public IActionResult Get()
        {
            var empresas = _conexion.getDataEmpresasGeneral();
            return Ok(empresas);
        }

        // GET api/<TableroController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var grafico = _conexion.getDataGraficoGeneralByIdCliente(id);
            var personaResultado = _conexion.getDataPersonasResultadosByIdCliente(id);
            var procesoLogro = _conexion.getDataProcesosLogrosByIdCliente(id);

            double acre = personaResultado.Where(x => x.tipoResultado == "Acreditado").FirstOrDefault().cantidadResultado;
            double acreRef = personaResultado.Where(x => x.tipoResultado == "Acreditado").FirstOrDefault().referencia;
            double noAcre = personaResultado.Where(x => x.tipoResultado != "Acreditado" && x.tipoResultado != "Personas").Sum(s => s.cantidadResultado);
            double noAcreRef = personaResultado.Where(x => x.tipoResultado != "Acreditado" && x.tipoResultado != "Personas").Sum(s => s.referencia);
            double tot = acre + noAcre;
            double totRef = acreRef + noAcreRef;

            var distPerfiles = grafico.GroupBy(i => new { i.codPerfil, i.perfil }).Select(m => m.First()).ToList();
            var distFaena = grafico.Select(m => m.faena).Distinct().ToList<string>();

            List<GraficoTablero> tableroData = new List<GraficoTablero>();

            List<double> qFaena = new List<double>();
            List<int> qPerFaena = new List<int>();
            foreach (var perfil in distPerfiles)
            {
                var cantidad = grafico.FirstOrDefault(x => x.perfil == perfil.perfil);
                qFaena.Add(cantidad != null ? cantidad.porcentajeCumplimiento : 0.0);
                qPerFaena.Add(0);
            }

            tableroData.Add(new GraficoTablero() { NombreFaena = "REFERENCIA", cantidadProcesos = qPerFaena, porcentajeCumplimiento = qFaena });

            foreach (var faena in distFaena)
            {
                List<int> cantidadFaena = new List<int>();
                List<double> porcentajeFaena = new List<double>();
                foreach (var perfil in distPerfiles)
                {
                    var cantidad = grafico.FirstOrDefault(x => x.perfil == perfil.perfil && x.faena == faena);
                    cantidadFaena.Add(cantidad != null ? cantidad.cantidad : 0);
                    porcentajeFaena.Add(cantidad != null ? cantidad.porcentajePerfil : 0);
                }

                tableroData.Add(new GraficoTablero() { NombreFaena = faena, cantidadProcesos = cantidadFaena, porcentajeCumplimiento = porcentajeFaena });
            }

            var result = new
            {
                Cliente = procesoLogro[0].cliente,
                Vigentes = procesoLogro[0].vigentes,
                Historicos = procesoLogro[0].noVigentes,
                Total = (procesoLogro[0].vigentes + procesoLogro[0].noVigentes),
                LogroEmpresa = procesoLogro[0].cumplimientoEmpresa,
                LogroIndustria = procesoLogro[0].cumplimientoIndustria,
                CantidadPerfiles = grafico.Select(m => m.perfil).Distinct().Count(),
                CantidadPersonas = personaResultado.Where(x => x.tipoResultado == "Personas").FirstOrDefault().cantidadResultado,
                Acreditados = (int)Math.Round(acre * 100.0 / tot),
                NoAcreditados = (int)Math.Round(noAcre * 100.0 / tot),
                AcreditadosRef = (int)Math.Round(acreRef * 100.0 / totRef),
                NoAcreditadosRef = (int)Math.Round(noAcreRef * 100.0 / totRef),
                Perfiles = distPerfiles,
                DataGrafico = tableroData,
            };

            return Ok(result);
        }

   

        //custom get Methods

        [Route("[action]/{idCliente}")]
        [HttpGet]
        public IActionResult GetPerfilBrechaById(int idCliente)
        {

            var idUsuario = this.GetIdUser();

            AutorizacionHelper helper = new();

            if (!helper.EstaAutorizado(idUsuario, EnumPermisos.TableroGestion))
            {
                return Unauthorized();
            }

            var perfilBrecha = _conexion.getDataPerfilBrechaByIdCliente(idCliente);

            var unePerfil = perfilBrecha.GroupBy(x => x.Codigo)
                .Select(g => new
                {
                    perfil = g.Select(x => x.Perfil).FirstOrDefault(),
                    cantidadBrechas = g.Sum(x => x.Cantidad)
                }).ToList().OrderByDescending(x => x.cantidadBrechas);

            var uneBrecha = perfilBrecha.OrderByDescending(x => x.Cantidad);

            var resultado = new
            {
                Perfil = unePerfil,
                Brecha = uneBrecha
            };

            return Ok(resultado);
        }

        [Route("[action]/{idCliente}/{codPerfil}")]
        [HttpGet]
        public IActionResult GetPerfilCriteriosById(int idCliente, string codPerfil)
        {
            var dataCriterio = _conexion.getDataCriterioGraficoByIdCliente(idCliente, codPerfil);
            var dataCriterioGeneral = _conexion.getDataCriterioPCTGraficoByIdPerfil(codPerfil);
            var dataLogroGeneral = _conexion.getDataLogroInstrumentoByIdCliente(idCliente, codPerfil).FirstOrDefault();

            var distFaena = dataCriterio.Select(m => m.faena).Distinct().ToList<string>();


            List<Criterio> dataSalidaCriterio = new List<Criterio>();

            foreach (var faena in distFaena)
            {
                List<GraficoCriterio> dataGraficoCriterio = new List<GraficoCriterio>();
                foreach (var item in dataCriterio.Where(x => x.faena == faena))
                {
                    var general = dataCriterioGeneral.Where(x => x.codCriterio == item.codCriterio).ToList().FirstOrDefault();
                    dataGraficoCriterio.Add(new GraficoCriterio()
                    {
                        codCriterio = item.codCriterio,
                        codActividad = item.codActividad,
                        correctas = item.cantidadB,
                        incorrectas = item.cantidadM,
                        total = (item.cantidadM + item.cantidadB),
                        cumplimientoIndustria = ((general.cantidadB * 100.0) / (general.cantidadM + general.cantidadB))
                    });
                }
                dataSalidaCriterio.Add(new Criterio { faena = faena, dataGrafico = dataGraficoCriterio });
            }

            var logroPerfil = new
            {
                pctInsuficiente = ((dataLogroGeneral.PCT_insuficiente * 100.0) / dataLogroGeneral.totalGeneral),
                pctSuficiente = ((dataLogroGeneral.PCT_Suficiente * 100.0) / dataLogroGeneral.totalGeneral),
                pctSuperior = ((dataLogroGeneral.PCT_Superior * 100.0) / dataLogroGeneral.totalGeneral),
                protSuficiente = ((dataLogroGeneral.PROT_Suficiente * 100.0) / dataLogroGeneral.totalGeneral),
                protSuperior = ((dataLogroGeneral.PROT_Superior * 100.0) / dataLogroGeneral.totalGeneral),
                eiInsuficiente = ((dataLogroGeneral.EI_insuficiente * 100.0) / dataLogroGeneral.totalGeneral),
                eiSuficiente = ((dataLogroGeneral.EI_Suficiente * 100.0) / dataLogroGeneral.totalGeneral),
                eiSuperior = ((dataLogroGeneral.EI_Superior * 100.0) / dataLogroGeneral.totalGeneral),
            };

            var salida = new
            {
                logro = logroPerfil,
                dataGrafico = dataSalidaCriterio
            };


            return Ok(salida);
        }

        [Route("[action]/{idCliente}")]
        [HttpGet]
        public IActionResult GetFaenaPerfilCandidatoByCliente(int idCliente)
        {
            var faenaPerfilCandidato = _conexion.getDataFaenaPerfilCandidatoByIdCliente(idCliente);

            return Ok(faenaPerfilCandidato);
        }

        [Route("[action]/{idCliente}/{codPerfil}/{run}/{faena}")]
        [HttpGet]
        public IActionResult GetPerfilCriteriosCandidatoById(int idCliente, string codPerfil, string run, string faena)
        {
            var dataCriterioPCT = _conexion.getDataCriterioPCTGraficoCandidatoByIdCliente(idCliente, codPerfil, run, faena);
            var dataCriterioPROT = _conexion.getDataCriterioPROTGraficoCandidatoByIdCliente(idCliente, codPerfil, run, faena);
            var dataCriterioGeneralPCT = _conexion.getDataCriterioPCTGraficoByIdPerfil(codPerfil);
            var dataCriterioGeneralPROT = _conexion.getDataCriterioPROTGraficoByIdPerfil(codPerfil);
            var procesoLogroCandidato = _conexion.getDataLogroCandidatoByIdCliente(idCliente, codPerfil, run, faena).ToList().FirstOrDefault();

            var dataBrecha = _conexion.getDataPerfilBrechaCandidatoByIdCliente(idCliente, codPerfil, run, faena);

            List<GraficoCriterio> dataGraficoCriterioPCT = new List<GraficoCriterio>();
            foreach (var item in dataCriterioPCT)
            {
                var general = dataCriterioGeneralPCT.Where(x => x.codCriterio == item.codCriterio).ToList().FirstOrDefault();
                dataGraficoCriterioPCT.Add(new GraficoCriterio()
                {
                    codCriterio = item.codCriterio,
                    codActividad = item.codActividad,
                    correctas = item.cantidadB,
                    incorrectas = item.cantidadM,
                    total = (item.cantidadM + item.cantidadB),
                    cumplimientoIndustria = ((general.cantidadB * 100.0) / (general.cantidadM + general.cantidadB))
                });
            }


            List<GraficoCriterio> dataGraficoCriterioPROT = new List<GraficoCriterio>();
            foreach (var item in dataCriterioPROT)
            {
                var general = dataCriterioGeneralPROT.Where(x => x.codCriterio == item.codCriterio).ToList().FirstOrDefault();
                dataGraficoCriterioPROT.Add(new GraficoCriterio()
                {
                    codCriterio = item.codCriterio,
                    codActividad = item.codActividad,
                    correctas = item.cantidadB,
                    incorrectas = item.cantidadM,
                    total = (item.cantidadM + item.cantidadB),
                    cumplimientoIndustria = ((general.cantidadB * 100.0) / (general.cantidadM + general.cantidadB))
                });
            }

            var salida = new
            {
                brecha = dataBrecha,
                dataGraficoPCT = dataGraficoCriterioPCT,
                dataGraficoPROT = dataGraficoCriterioPROT,
                logroCandidato = procesoLogroCandidato
            };

            return Ok(salida);
        }

    }
}
