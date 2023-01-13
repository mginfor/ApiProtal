using Entities.DbModels;
using Entities.EPModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IProcesoService
    {

        public List<Proceso> getProcesosByIdInforme(int idInforme);
        public List<Proceso> getProcesosByIdEvaluacion(int idEvaluacion);
        public List<Proceso> getProcesosByHash(string hash);
        public List<Proceso> getProcesosByProcesoEP(ProcesoEP proceso);
        public List<BrechaPortal> getbrechasByHash(int idEvaluacion);
        public List<ProcesoReportabilidad> GetProcesosReportabilidad();





    }
}
