using Entities.DbModels;
using Entities.EPModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IProcesoService
    {

        public List<ProcesoInforme> getProcesosByIdInforme(int idInforme);
        public List<Proceso> getProcesosByIdEvaluacion(int idEvaluacion);
        public List<Proceso> getProcesosByHash(string hash);
        public List<Proceso> getProcesosByProcesoEP(ProcesoEP proceso);
        public List<BrechaPortal> getbrechasByHash(int idEvaluacion);
        public List<ProcesoReportabilidad> GetProcesosReportabilidad();
        public List<ProcesoReportabilidad> GetProcesosReportabilidadUltramar2();
        public List<ProyectosyContratos> GetDetallesClienteProyecto(int crrIdCliente);
        public List<ProcesoReportabilidad> GetProcesosReportabilidadUltramar(int crrProyectoContrato);
        
       public List<Proceso> getProcesosByProcesoTratamiento(ProcesoEP proceso);
        public List<Proceso> getProcesosByIdEvaluacionTratamiento(int idEvaluacion);

        public List<ProcesoInformeValidador> getProcesosByIdInformeNuevo(string idInforme, string runCandidato, int idCliente);






    }
}
