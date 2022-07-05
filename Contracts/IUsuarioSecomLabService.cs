using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;

namespace Contracts
{
    public interface IUsuarioSecomLabService:IRepositoryBase<UsuarioSecomLab>
    {
        AuthenticateResponseSecomlab Authenticate(AuthenticateRequestSecomlab model);
    }
}
