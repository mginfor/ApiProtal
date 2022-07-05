using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;

namespace Services
{
    public class LogService : RepositoryBase<Log>, ILogService
    {
        public LogService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
    }
}
