using api.Helpers;
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
        private readonly JWT _jwt;

        public UsuarioPortalService(RepositoryContext repositoryContext, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor, JWT jwt) : base(repositoryContext)
        {
            _jwt = jwt;
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
       
                var ahora = DateTime.UtcNow;
                if(user.UltimoEnvioCodigo.HasValue && ahora.Subtract(user.UltimoEnvioCodigo.Value).TotalMinutes < 2)
                {
                   
                    return null;
           
                };



                user.clave = generarCodigo();
                user.UltimoEnvioCodigo = ahora;
                update(user);
            }
            return user;
        }



        public AuthenticateResponsePortal Authenticate(AuthenticateRequestPortal model)
        {

            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();


            var user = findByCondition(x => x.run.ToUpper() == model.RutUsuario.ToUpper() &&
                                      (x.cliente.run.ToString() + x.cliente.digito).ToUpper() == model.RutCliente.ToUpper(),
                                        //&& x.clave == model.Codigo,
                                       new string[] { "cliente", "rol" })
                                      .ToList()
                                      .FirstOrDefault();
            //  usuario y empresa invalido

           

            if (user == null)
            {
             
                return null; 
            }

            if (user.estado == 0 && user.fechaBloqueo.HasValue)
            {
                //desbloqueo despues de 30 min
                var tiempoBloqueo = DateTime.Now - user.fechaBloqueo.Value;
                if (tiempoBloqueo.TotalMinutes > 30)
                {
                    user.estado = 1;
                    user.intentos = 0;
                    this._repositoryContext.UsuarioPortals.Update(user);
                }
               
            }
            // contraseña invalida
            if (user.clave != model.Codigo)
            {
                user.intentos++;
                var mensaje = "Usuario o contraseña incorrecta, al tercer intento usuario sera bloqueado";
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
                    mensaje = "Usuario bloqueado";

                }
                this._repositoryContext.UsuarioPortals.Update(user);
                this._repositoryContext.SaveChanges();

                return new AuthenticateResponsePortal() { Mensaje=mensaje};
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

            JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            //var token = generateJwtToken(user, permisos, out var fechaExpiracion); 


            var logLogin = new LogLogin
            {
                id_usuario = user.id,
                nombreUsuario = user.nombreUsuario,
                cargoUsuario = user.cargo,
                token = token,
                fecha_creacion = DateTime.Now,

               
            };

            this._repositoryContext.LogLogins.Add(logLogin);

            this._RepositoryContext.SaveChanges();
            return new AuthenticateResponsePortal(user, token, permisos); 
        }


        private JwtSecurityToken CreateJwtToken(UsuarioPortal usuario)
        {
            var rol = usuario.rol;
            var rolesClaims = new List<Claim>
                {
                    new Claim("roles", rol.nombreRol)
                };
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email,usuario.correo),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim("uid",usuario.id.ToString())
            }.Union(rolesClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;

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
