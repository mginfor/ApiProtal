using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;

namespace Services
{
    public class ResultadoService : RepositoryBase<Resultado>, IResultadoService
    {
        public ResultadoService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
    }
}
