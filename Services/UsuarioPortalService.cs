using Contracts;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Http;
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
        private readonly RepositoryContext _repositoryContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioPortalService(RepositoryContext repositoryContext, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor) : base(repositoryContext)
        {
            _appSettings = appSettings.Value;
            _repositoryContext = repositoryContext;
            _httpContextAccessor = httpContextAccessor;
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

            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();


            var user = findByCondition(x => x.run.ToUpper() == model.RutUsuario.ToUpper() &&
                                      (x.cliente.run.ToString() + x.cliente.digito).ToUpper() == model.RutCliente.ToUpper() &&
                                       x.clave == model.Codigo,
                                       new string[] { "cliente", "rol" })
                                      .ToList()
                                      .FirstOrDefault();
            // return null if user not found
            if (user == null)
            {
                throw new AuthenticationException("Usuario o Contraseña incorrectos");
            }

            if (user.estado == 0 && user.fechaBloqueo.HasValue)
            {
                var tiempoBloqueo = DateTime.Now - user.fechaBloqueo.Value;
                if (tiempoBloqueo.TotalMinutes > 30)
                {
                    user.estado = 1;
                    user.intentos = 0;
                }
                else
                {
                    throw new AuthenticationException("Su cuenta está bloqueada y se desbloqueará después de 30 minutos.");
                }
            }

            if (user.clave != model.Codigo)
            {
                user.intentos++;

                if(user.intentos >= 3)
                {
                    user.estado = 0;
                    user.fechaBloqueo = DateTime.Now;


                    var logBloqueo = new LogBloqueo
                    {
                        IdUsuario = user.id,
                        NombreUsuario = user.nombreUsuario,
                        CargoUsuario = user.cargo,
                        CrrCliente = user.idCliente,
                        FechaBloqueo = DateTime.Now,
                        DireccionIP = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString()
                    };

                    this._repositoryContext.LogBloqueos.Add(logBloqueo);
                    throw new AuthenticationException("Su cuenta ha sido bloqueada debido a múltiples intentos fallidos.");

                }

                this._repositoryContext.SaveChanges();

                return null;
            }

            user.intentos = 0;
            user.estado = 1; 
            this._RepositoryContext.SaveChanges();


            var permisos = this._RepositoryContext.RolPermisos
                .Include(x => x.rol)
                .Include(x => x.permiso)
                .Where(x => x.rol.id == user.idRol)
                .Select(x => x.permiso).ToList();
            // authentication successful so generate jwt token
            var token = generateJwtToken(user, out var fechaExpiracion);

          
            var logLogin = new LogLogin
            {
                id_usuario = user.id,
                nombreUsuario = user.nombreUsuario,
                cargoUsuario = user.cargo,
                token = token,
                fecha_creacion = DateTime.Now,
                fecha_expiracion = fechaExpiracion
               
            };

            this._repositoryContext.LogLogins.Add(logLogin);

            this._RepositoryContext.SaveChanges();
            return new AuthenticateResponsePortal(user, token, permisos); 
        }

        private string generateJwtToken(UsuarioPortal user, out DateTime fechaExpiracion)
        {
            fechaExpiracion = DateTime.UtcNow.AddHours(8);
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
            //var includes = new string[] { "cliente", "rol" };
            //var user = findByCondition(x => x.id == Id, includes).ToList().First();
            //return user;

            var user = _RepositoryContext.UsuarioPortals
                .Include(u => u.rol)
                .Include(u => u.cliente)
                .FirstOrDefault(u => u.id == Id);

            return user;

        }


        public List<UsuarioPortal> GetAllUsuario()
        {


            var usuario = _RepositoryContext.UsuarioPortals
                .Include(u => u.rol)
                .Include(u => u.cliente)
                .ToList();

            return usuario;
        }


        private int generarCodigo()
        {
            Random rnd = new();
            return rnd.Next(100000, 999999);
        }

        public bool EstaAutorizado(int idUsuario, string constantePermisos)
        {



            var userRol = _repositoryContext.UsuarioPortals
                 .Where(u => u.id == idUsuario)
                 .Include(u => u.rol)
                 .FirstOrDefault();

            var permisos = _repositoryContext.RolPermisos
               .Include(x => x.rol)
               .Include(x => x.permiso)
               .Where(x => x.rol.id == userRol.idRol)
               .Select(x => x.permiso).ToList();

            var estaAutorizado = permisos.Exists(x => x.NombrePermiso.Equals(constantePermisos));

            return estaAutorizado;
        }


        public AuthenticateResponsePortal GetUserByToken(string token)
        {
            // ir a buscar por token y retornar la misma info que retorna  el login 
            var log = _repositoryContext.LogLogins
                .Where(x => x.token == token && x.fecha_expiracion > DateTime.Now)
                .Include(x => x.usuario )
                .ThenInclude(x => x.rol)
                .Include(x => x.usuario)
                .ThenInclude(x =>x.cliente)
                .FirstOrDefault();

      

            var permisos = _repositoryContext.RolPermisos
              .Include(x => x.rol)
              .Include(x => x.permiso)
              .Where(x => x.rol.id == log.usuario.rol.id)
              .Select(x => x.permiso).ToList();


            return new AuthenticateResponsePortal(log.usuario, log.token, permisos);
        }
        
    }


    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message)
        {
        }
    }
}
