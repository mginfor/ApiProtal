using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EstadoProcesoService : RepositoryBase<EstadoProceso>, IEstadoProcesoService
    {
        public EstadoProcesoService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }

        public List<EstadoProceso> GetAllEstadoProcesos() {
            return findAll().ToList();
        }
    }
}
