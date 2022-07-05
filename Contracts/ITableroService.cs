using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface ITableroService
    {
        public List<Empresas> getDataEmpresasGeneral();
        public List<GraficoGeneral> getDataGraficoGeneralByIdCliente(int idCliente);
        public List<PersonasResultado> getDataPersonasResultadosByIdCliente(int idCliente);
        public List<ProcesosLogros> getDataProcesosLogrosByIdCliente(int idCliente);
        public List<PerfilBrechas> getDataPerfilBrechaByIdCliente(int idCliente);
        public List<CriterioGrafico> getDataCriterioGraficoByIdCliente(int idCliente, string codPerfil);
        public List<LogroInstrumentos> getDataLogroInstrumentoByIdCliente(int idCliente, string codPerfil);
        public List<CriterioGrafico> getDataCriterioPCTGraficoByIdPerfil(string codPerfil);
        public List<CriterioGrafico> getDataCriterioPROTGraficoByIdPerfil(string codPerfil);
        public List<FaenaPerfilCandidato> getDataFaenaPerfilCandidatoByIdCliente(int idCliente);
        public List<CriterioGrafico> getDataCriterioPCTGraficoCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena);
        public List<CriterioGrafico> getDataCriterioPROTGraficoCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena);
        public List<PerfilBrechas> getDataPerfilBrechaCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena);
        public List<ProcesoLogroCandidato> getDataLogroCandidatoByIdCliente(int idCliente, string codPerfil, string run, string faena);
    }
}
