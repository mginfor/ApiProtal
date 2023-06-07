using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using Services.Generic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
namespace Services
{
    public class UsuarioPortalService : RepositoryBase<UsuarioPortal>, IUsuarioPortalService
    {
        private readonly AppSettings _appSettings;
        public UsuarioPortalService(RepositoryContext repositoryContext, IOptions<AppSettings> appSettings) : base(repositoryContext)
        {
            _appSettings = appSettings.Value;
        }


        public UsuarioPortal PreLogin(AuthenticateRequestPortal model)
        {
            var user = findByCondition(x => x.run == model.RutUsuario &&
                                       (x.cliente.run.ToString() + x.cliente.digito) == model.RutCliente
                                       , "cliente")
                                      .ToList()
                                      .FirstOrDefault();
            if (user != null)
            {
                user.clave = generarCodigo();
                update(user);
            }
            return user;
        }
        public AuthenticateResponsePortal Authenticate(AuthenticateRequestPortal model)
        {
            var user = findByCondition(x => x.run.ToUpper() == model.RutUsuario.ToUpper() &&
                                      (x.cliente.run.ToString() + x.cliente.digito).ToUpper() == model.RutCliente.ToUpper() &&
                                       x.clave == model.Codigo,
                                       new string[] { "cliente", "rol" })
                                      .ToList()
                                      .FirstOrDefault();
            // return null if user not found
            if (user == null) return null;
            var permisos = this._RepositoryContext.RolPermisos
                .Include(x => x.rol)
                .Include(x => x.permiso)
                .Where(x => x.rol.id == user.idRol)
                .Select(x => x.permiso).ToList();
            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponsePortal(user, token, permisos);
        }

        private string generateJwtToken(UsuarioPortal user)
        {
            // generate token that is valid for 8 HOURS
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.id.ToString()) }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public UsuarioPortal GetById(int Id)
        {
            var includes = new string[] { "cliente", "rol" };
            var user = findByCondition(x => x.id == Id, includes).ToList().First();
            return user;

        }


        public List<UsuarioPortal> GetAllUsuario()
        {


            return this.findAll().ToList();
        }


        private int generarCodigo()
        {
            Random rnd = new();
            return rnd.Next(100000, 999999);
        }


    }
}
