using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IEvaluacionEIService:IRepositoryBase<EvaluacionEI>
    {
        public List<EvaluacionEI> getBrechasByIdEvaluacion(int idEvaluacion);
        public bool addDocumentoInBrecha(int idBrecha, int idDocumento);

    }
}
