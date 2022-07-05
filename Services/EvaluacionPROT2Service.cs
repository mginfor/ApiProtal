using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;

namespace Services
{
    public class EvaluacionPROT2Service:RepositoryBase<EvaluacionPROT2>,IEvaluacionPROT2Service
    {
        public EvaluacionPROT2Service(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
    }
}
