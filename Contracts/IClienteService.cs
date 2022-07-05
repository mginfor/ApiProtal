using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IClienteService : IRepositoryBase<Cliente>
    {
        public Cliente getClienteServicio(int idCliente);
        public List<Cliente> getAllCliente();

        public Cliente getClienteByidCliente(int idCliente);
    }
}
