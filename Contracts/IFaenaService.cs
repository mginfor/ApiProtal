using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IFaenaService : IRepositoryBase<Faena>
    {
        public List<Faena> getAllFaenas();
    }
}
