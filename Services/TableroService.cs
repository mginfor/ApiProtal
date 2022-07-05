using Contracts;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class TableroService : ITableroService
    {
        private RepositoryContext db;
        public TableroService()
        {
            db = new RepositoryContext();
        }
        public List<Empresas> getDataEmpresasGeneral()
        {
            var query = "call mgschema.getDataEmpresa();";

            return db.dataEmpresas
                .FromSqlRaw(query)
                .ToList();
        }

        public List<GraficoGeneral> getDataGraficoGeneralByIdCliente(int idCliente)
        {
            var query = "call mgschema.getGraficoGeneral({0});";

            return db.dataGraficos
                .FromSqlRaw(query, idCliente)
                .ToList();
        }

        public List<PersonasResultado> getDataPersonasResultadosByIdCliente(int idCliente)
        {
            var query = "call mgschema.getPersonasResultados({0});";

            return db.dataPersonasResultados
                .FromSqlRaw(query, idCliente)
                .ToList();
        }

        public List<ProcesosLogros> getDataProcesosLogrosByIdCliente(int idCliente)
        {
            var query = "call mgschema.getProcesosLogros({0});";

            return db.dataProcesosLogros
                .FromSqlRaw(query, idCliente)
                .ToList();
        }

        public List<PerfilBrechas> getDataPerfilBrechaByIdCliente(int idCliente)
        {
            var query = "call mgschema.getPerfilBrechas({0});";

            return db.dataPerfilBrechas
                .FromSqlRaw(query, idCliente)
                .ToList();
        }

        public List<CriterioGrafico> getDataCriterioGraficoByIdCliente(int idCliente, string codPerfil)
        {
            var query = "call mgschema.getDataCriterio({0},{1});";

            return db.dataCriterioGraficos
                .FromSqlRaw(query, idCliente, codPerfil)
                .ToList();
        }

        public List<LogroInstrumentos> getDataLogroInstrumentoByIdCliente(int idCliente, string codPerfil)
        {
            var query = "call mgschema.getDataLogroInstrumento({0},{1});";

            return db.dataLogroInstrumentos
                .FromSqlRaw(query, idCliente, codPerfil)
                .ToList();
        }
        public List<CriterioGrafico> getDataCriterioPCTGraficoByIdPerfil(string codPerfil)
        {
            var query = "call mgschema.getDataCriterioGeneralPCT({0});";

            return db.dataCriterioGraficos
                .FromSqlRaw(query, codPerfil)
                .ToList();
        }
        public List<CriterioGrafico> getDataCriterioPROTGraficoByIdPerfil(string codPerfil)
        {
            var query = "call mgschema.getDataCriterioGeneralPROT({0});";

            return db.dataCriterioGraficos
                .FromSqlRaw(query, codPerfil)
                .ToList();
        }
        public List<FaenaPerfilCandidato> getDataFaenaPerfilCandidatoByIdCliente(int idCliente)
        {
            var query = "call mgschema.getFaenaPerfilCandidato({0});";

            return db.dataFaenaPerfilCandidatos
                .FromSqlRaw(query, idCliente)
                .ToList();
        }

        public List<CriterioGrafico> getDataCriterioPCTGraficoCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena)
        {
            var query = "call mgschema.getDataCriterioPCTCliFaeCan({0},{1},{2},{3});";

            return db.dataCriterioGraficos
                .FromSqlRaw(query, idCliente, codPerfil, run, faena)
                .ToList();
        }
        public List<CriterioGrafico> getDataCriterioPROTGraficoCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena)
        {
            var query = "call mgschema.getDataCriterioPROTCliFaeCan({0},{1},{2},{3});";

            return db.dataCriterioGraficos
                .FromSqlRaw(query, idCliente, codPerfil, run, faena)
                .ToList();
        }
        public List<PerfilBrechas> getDataPerfilBrechaCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena)
        {
            var query = "call mgschema.getPerfilBrechasCliFaeCan({0},{1},{2},{3});";

            return db.dataPerfilBrechas
                .FromSqlRaw(query, idCliente, codPerfil, run, faena)
                .ToList();
        }

        public List<ProcesoLogroCandidato> getDataLogroCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena)
        {
            var query = "call mgschema.getDataLogroCandidato({0},{1},{2},{3});";

            return db.dataProcesoLogroCandidatos
                .FromSqlRaw(query, idCliente, codPerfil, run, faena)
                .ToList();
        }
    }
}
