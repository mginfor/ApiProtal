using Contracts.Generic;
using Entities.DbModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IPerfilService : IRepositoryBase<Perfil>
    {
        public List<Perfil> getAllPerfiles();
    }
}
