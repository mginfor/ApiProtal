using Contracts;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;



namespace Services
{
    public class ClienteService : RepositoryBase<Cliente>, IClienteService
    {
        private RepositoryContext db;

        public ClienteService(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            db = new RepositoryContext();

        }

        public Cliente getClienteServicio(int idCliente)
        {
            return this.findByCondition(x => x.id == idCliente, "serviciosVinculados").ToList().FirstOrDefault();
        }
        
        public List<Cliente> getAllCliente()
        { 
            return this.findAll().ToList();
        }

        public Cliente getClienteByidCliente(int idCliente)
        {

            return this.findByCondition(x => x.id == idCliente).ToList().FirstOrDefault();


        }




    }
}
