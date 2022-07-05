using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IEvaluacionExportarService : IRepositoryBase<Evaluacion>
    {
        public List<EvaluacionExportarEP> getAllEvaluationsByCliente(int idCliente);
    }
}
