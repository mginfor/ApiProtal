using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using System.Collections.Generic;

namespace Contracts
{
    public interface IUsuarioPortalService : IRepositoryBase<Entities.DbModels.UsuarioPortal>
    {
        AuthenticateResponsePortal Authenticate(AuthenticateRequestPortal model);

        public Entities.DbModels.UsuarioPortal GetById(int Id);
        public UsuarioPortal PreLogin(AuthenticateRequestPortal model);
        public List<UsuarioPortal> GetAllUsuario();
        public bool EstaAutorizado(int idUsuario, string constantePermisos);
        public AuthenticateResponsePortal GetUserByToken(string token);


    }
}
