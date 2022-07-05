using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class ServicioVinculadoService : RepositoryBase<ServiciosVinculados>, IServicioVinculadoService
    {
        private readonly AppSettings _appSettings;

        public ServicioVinculadoService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
      
        public ServiciosVinculados getServicioPorIdCliente(int idCliente)
        {
            var nuevo= findByCondition(x => x.idCliente == idCliente ).ToList().FirstOrDefault();
            return nuevo;
        }
    }
   
}
