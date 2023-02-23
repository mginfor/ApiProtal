using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IEvaluacionPROTService:IRepositoryBase<EvaluacionPROT>
    {
        public List<EvaluacionPROT> getBrechasByIdEvaluacion(int idEvaluacion);
        public bool addDocumentoInBrecha(int idBrecha, int idDocumento);


    }
}
