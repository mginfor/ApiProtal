using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IEvaluacionPCTService:IRepositoryBase<EvaluacionPCT>
    {
        public List<EvaluacionPCT> getBrechasByIdEvaluacion(int idEvaluacion);
        public bool addDocumentoInBrecha(int idBrecha, int idDocumento);
    }
}
