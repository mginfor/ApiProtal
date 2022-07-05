using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;


namespace Services
{
    public class SectorService : RepositoryBase<Sector>, ISectorService
    {
        public SectorService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
        public List<Sector> getAllSectores()
        {
            return this.findByCondition(x => x.vigencia != 0).ToList();
        }
    }
}
