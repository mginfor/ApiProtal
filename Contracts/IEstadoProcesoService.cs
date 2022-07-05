using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IEstadoProcesoService : IRepositoryBase<EstadoProceso>
    {
        public List<EstadoProceso> GetAllEstadoProcesos();


    }
}
