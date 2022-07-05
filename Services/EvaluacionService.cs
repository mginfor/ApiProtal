using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;

namespace Services
{
    public class EvaluacionService : RepositoryBase<Evaluacion>, IEvaluacionService
    {
        public EvaluacionService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
    }
}
