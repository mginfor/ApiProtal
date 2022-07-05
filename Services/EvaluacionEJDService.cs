using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;

namespace Services
{
    public class EvaluacionEJDService:RepositoryBase<EvaluacionEJD>,IEvaluacionEJDService
    {
        public EvaluacionEJDService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
    }
}
