using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface ISectorService : IRepositoryBase<Sector>
    {
        public List<Sector> getAllSectores();
    }
}
