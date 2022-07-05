using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class FaenaService : RepositoryBase<Faena>, IFaenaService
    {
        public FaenaService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
        public List<Faena> getAllFaenas()
        {
            return this.findAll().ToList();
        }
    }
}
