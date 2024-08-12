using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        Task<UsuarioPortal> GetByRefreshTokenAsync(string refreshToken);

        Task<DatosUsuarios> LoginAsync(AuthenticateRequestPortal model);

        Task<DatosUsuarios> Refreshtoken(string refreshToken);


        Task<DatosUsuarios> ValidateTokenAsync(string token);


    }
}
