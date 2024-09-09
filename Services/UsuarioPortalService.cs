using api.Helpers;
using Contracts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Presentation;
using DocumentFormat.OpenXml.Spreadsheet;
using Entities.DbModels;
using Entities.EPModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Graph.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.SharePoint.Client;
using Persistence;
using Services.Generic;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;



namespace Services
{
    public class UsuarioPortalService : RepositoryBase<UsuarioPortal>, IUsuarioPortalService
    {
        private readonly AppSettings _appSettings;
        private readonly RepositoryContext _repositoryContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JWT _jwt;
        private IServicioVinculadoService _servicioVinculadoService;
        private IClienteService _clienteService;

        public UsuarioPortalService(RepositoryContext repositoryContext, IOptions<AppSettings> appSettings, IHttpContextAccessor httpContextAccessor, JWT jwt, IServicioVinculadoService servicioVinculadoService,
            IClienteService clienteService) : base(repositoryContext)
        {
            _jwt = jwt;
            _appSettings = appSettings.Value;
            _repositoryContext = repositoryContext;
            _httpContextAccessor = httpContextAccessor;
            _servicioVinculadoService = servicioVinculadoService;
            _clienteService = clienteService;

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

                user.Pass = GenerarCodigo();
                user.UltimoEnvioCodigo = ahora;
                update(user);
            }
            return user;
        }



        public async Task<DatosUsuarios> LoginAsync(AuthenticateRequestPortal model)
        {
            var ipAddress = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            DatosUsuarios datosUsuarioDto = new DatosUsuarios();

            // Buscar usuario basado en rut y clave
            var user = await _repositoryContext.UsuarioPortals
                             .Include(u => u.rol)
                             .Include(u => u.cliente)
                             .Include(u => u.RefreshTokens)
                              .FirstOrDefaultAsync(x => x.run.ToUpper() == model.RutUsuario.ToUpper() &&
                               (x.cliente.run.ToString() + x.cliente.digito).ToUpper() == model.RutCliente.ToUpper());

            // Verificar si el usuario no existe
            if (user == null)
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = "Usuario o contraseña inválida";
                return datosUsuarioDto;
            }

            // Verificar contraseña
            if (user.Pass != model.Codigo)
            {
                user.intentos++;
                var mensaje = "Usuario o contraseña incorrecta, al tercer intento usuario será bloqueado";
                if (user.intentos >= 3)
                {
                    user.estado = 0;
                    user.fechaBloqueo = DateTime.Now;

                    // Registrar bloqueo
                    var logBloqueo = new LogBloqueo
                    {
                        IdUsuario = user.id,
                        NombreUsuario = user.nombreUsuario,
                        CargoUsuario = user.cargo,
                        CrrCliente = user.idCliente,
                        FechaBloqueo = DateTime.Now,
                        DireccionIP = ipAddress
                    };

                    _repositoryContext.LogBloqueos.Add(logBloqueo);
                    mensaje = "Usuario bloqueado";
                }

                _repositoryContext.UsuarioPortals.Update(user);
                await _repositoryContext.SaveChangesAsync();

                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = mensaje;
                return datosUsuarioDto;
            }

            // Restablecer intentos y estado si la contraseña es correcta
            user.intentos = 0;
            user.estado = 1;
            _repositoryContext.UsuarioPortals.Update(user);
            await _repositoryContext.SaveChangesAsync();

            // Verificar si el usuario está activo
            if (user.activo == 0)
            {
                datosUsuarioDto.EstaAutenticado = false;
                datosUsuarioDto.Mensaje = "Usuario bloqueado";
                return datosUsuarioDto;
            }
            // Obtener información adicional
            var serviciosVinculados = _servicioVinculadoService.getServicioPorIdCliente(user.idCliente);
            var informacionCliente = _clienteService.getClienteByidCliente(user.idCliente);

            var rolPermisos = this._repositoryContext.RolPermisos
                .Include(x => x.permiso)
                .Where(x => x.rol.id == user.idRol)
                .Select(x => new
                {
                    Id = x.permiso.id,
                    NombrePermiso = x.permiso.NombrePermiso
                })
                 .ToList();

            // Crear JWT y Refresh Token
            datosUsuarioDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(user);
            datosUsuarioDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            datosUsuarioDto.Id = user.id;
            datosUsuarioDto.NombreUsuario = user.nombreUsuario + " " + user.apellidoUsuario;
            datosUsuarioDto.IdCliente = user.idCliente;
            datosUsuarioDto.cargo = user.cargo;
            datosUsuarioDto.zona = user.zona;
            datosUsuarioDto.nombrecliente = user.cliente?.nombre;
            datosUsuarioDto.Correo = user.correo;
            datosUsuarioDto.Rol = user.rol;
            datosUsuarioDto.UrlServicio = serviciosVinculados?.urlServicio ?? "";
            datosUsuarioDto.TipoNivelCliente = informacionCliente?.nivel?.ToString() ?? "";
            var Permisos = rolPermisos.Select(p => new Permisos
            {
                id = p.Id,
                NombrePermiso = p.NombrePermiso
            }).ToList();

            datosUsuarioDto.Permisos = Permisos;


          
            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activerefreshToken =  user.RefreshTokens.Where(a => a.IsActive).FirstOrDefault();
                datosUsuarioDto.RefreshToken = activerefreshToken.Token;
                datosUsuarioDto.RefreshTokenExpiration = activerefreshToken.Expires;
            }
            else
            {
                var refreshToken = CreateRefreshToken();
                datosUsuarioDto.RefreshToken = refreshToken.Token;
                datosUsuarioDto.RefreshTokenExpiration = refreshToken.Expires;
                refreshToken.idUsuario = user.id;
                _repositoryContext.RefreshToken.Add(refreshToken);
                _repositoryContext.UsuarioPortals.Update(user);
                await _repositoryContext.SaveChangesAsync();
            }

            // Registrar el login
            var logLogin = new LogLogin
            {
                id_usuario = user.id,
                nombreUsuario = user.nombreUsuario,
                cargoUsuario = user.cargo,
                token = datosUsuarioDto.Token,
                fecha_creacion = DateTime.Now,
            };


            _repositoryContext.LogLogins.Add(logLogin);
            await _repositoryContext.SaveChangesAsync();



            var logLoginToken = new LogToken
            {
                idUsuario = user.id,
                Token = datosUsuarioDto.Token,
                FechaIngreso = DateTime.Now,
                FechaExpiracion = jwtSecurityToken.ValidTo.ToUniversalTime(),
                Revocado = false,
                


            };


            _repositoryContext.LogLoginToken.Add(logLoginToken);
            await _repositoryContext.SaveChangesAsync();

            datosUsuarioDto.Mensaje = "Login exitoso";

            return datosUsuarioDto;


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
            if (user.Pass != model.Codigo)
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


        public async Task<DatosUsuarios> ValidateTokenAsync(string token)
        {
            var datosUsuariosDto = new DatosUsuarios();

            var tokenValido = await _repositoryContext.LogLoginToken.FirstOrDefaultAsync(x => !x.Revocado && DateTime.UtcNow < x.FechaExpiracion && x.Token == token);

            if (tokenValido != null)
            {
                var usuario = await GetIdUsuario(tokenValido.idUsuario);

                if (usuario.activo == 0)
                {
                    datosUsuariosDto.EstaAutenticado = false;
                    datosUsuariosDto.Mensaje = $"el ususario esta bloqueado.";
                    return datosUsuariosDto;
                }

                var rolPermisos = this._repositoryContext.RolPermisos
                .Include(x => x.permiso)
                .Where(x => x.rol.id == usuario.idRol)
                .Select(x => new
                {
                  Id = x.permiso.id,
                  NombrePermiso = x.permiso.NombrePermiso
                 })
               .ToList();


                datosUsuariosDto.EstaAutenticado = true;
                datosUsuariosDto.Id = usuario.id;
                JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
                datosUsuariosDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                datosUsuariosDto.Correo = usuario.correo;
                datosUsuariosDto.Rol = usuario.rol;
                var Permisos = rolPermisos.Select(p => new Permisos
                {
                    id = p.Id,
                    NombrePermiso = p.NombrePermiso
                }).ToList();

                datosUsuariosDto.Permisos = Permisos;

                if (usuario.RefreshTokens.Any(a => a.IsActive))
                {
                    var activeRefreshToken = usuario.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                    datosUsuariosDto.RefreshToken = activeRefreshToken.Token;
                    datosUsuariosDto.RefreshTokenExpiration = activeRefreshToken.Expires;
                }
                else
                {
                    var refreshToken = CreateRefreshToken();
                    datosUsuariosDto.RefreshToken = refreshToken.Token;
                    datosUsuariosDto.RefreshTokenExpiration = refreshToken.Expires;
                    refreshToken.idUsuario = usuario.id;
                    _repositoryContext.UsuarioPortals.Update(usuario);
                    await _repositoryContext.RefreshToken.AddAsync(refreshToken);
                    await _repositoryContext.SaveChangesAsync();

                }

                var LoginToken = new LogToken
                {
                    FechaIngreso = DateTime.UtcNow,
                    Token = datosUsuariosDto.Token,
                    FechaExpiracion = jwtSecurityToken.ValidTo.ToUniversalTime(),
                    Revocado = false,
                    idUsuario = usuario.id
                };
                _repositoryContext.LogLoginToken.Add(LoginToken);
                await _repositoryContext.SaveChangesAsync();

                return datosUsuariosDto;

            }

            datosUsuariosDto.EstaAutenticado = false;
            datosUsuariosDto.Mensaje = $"Token Invalido";
            return datosUsuariosDto;

        }


        public async Task<DatosUsuarios> Refreshtoken(string refreshToken)
        {
            var datosUsuariosDto = new DatosUsuarios();

            var usuario = await GetByRefreshTokenAsync(refreshToken);

            if (usuario != null)
            {
                datosUsuariosDto.EstaAutenticado = false;
                datosUsuariosDto.Mensaje = $"El token no pertenece a ningun Usuario";
                return datosUsuariosDto;
            }

            var refreshTokenBd = usuario.RefreshTokens.Single(x => x.Token == refreshToken);

      

            if (refreshTokenBd.IsActive)
            {
                datosUsuariosDto.EstaAutenticado = false;
                datosUsuariosDto.Mensaje = $"El token no esta activo";
                return datosUsuariosDto;
            }

            //revocamos el refresh token actual

            var rolPermisos = this._repositoryContext.RolPermisos
                .Include(x => x.permiso)
                .Where(x => x.rol.id == usuario.idRol)
                .Select(x => new
                {
                    Id = x.permiso.id,
                    NombrePermiso = x.permiso.NombrePermiso
                })
               .ToList();

            refreshTokenBd.Revoked = DateTime.UtcNow;

            var newRefreshToken = CreateRefreshToken();
            usuario.RefreshTokens.Add(newRefreshToken);
            _repositoryContext.UsuarioPortals.Update(usuario);
            await _repositoryContext.SaveChangesAsync();

            datosUsuariosDto.EstaAutenticado = true;
            JwtSecurityToken jwtSecurityToken = CreateJwtToken(usuario);
            datosUsuariosDto.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);    
            datosUsuariosDto.Correo = usuario.correo;
            datosUsuariosDto.Rol = usuario.rol;
            var Permisos = rolPermisos.Select(p => new Permisos
            {
                id = p.Id,
                NombrePermiso = p.NombrePermiso
            }).ToList();

            datosUsuariosDto.Permisos = Permisos;
            datosUsuariosDto.RefreshToken = newRefreshToken.Token;
            datosUsuariosDto.RefreshTokenExpiration = newRefreshToken.Expires;
            return datosUsuariosDto;

                                            
        }



        private RefreshToken CreateRefreshToken() 
        {
            var ramdomNumber = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(ramdomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(ramdomNumber),
                    Expires = DateTime.UtcNow.AddDays(10),
                    Created = DateTime.UtcNow
                };
            }
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


        //private int generarCodigo()
        //{
        //    Random rnd = new();
        //    return rnd.Next(100000, 999999);
        //}


        private string GenerarCodigo()
        {
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+[]{}|;:,.<>?";
            int longitud = 8;
            Random rnd = new();
            char[] codigo = new char[longitud];

            for (int i = 0; i < longitud; i++)
            {
                codigo[i] = caracteres[rnd.Next(caracteres.Length)];
            }

            return new string(codigo);
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

        public async Task<UsuarioPortal> GetByRefreshTokenAsync(string refreshToken)
        {
            return await _repositoryContext.UsuarioPortals
                         .Include(u => u.rol)
                         .Include(u => u.RefreshTokens)
                         .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken && t.IsActive));

        }

        public async Task<UsuarioPortal> GetByRefreshTokenAsync2(string refreshToken)
        {
            var utcNow = DateTime.UtcNow;

            return await _repositoryContext.UsuarioPortals
                         .Include(u => u.rol) 
                         .Include(u => u.RefreshTokens) 
                         .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t =>
                             t.Token == refreshToken &&  
                             t.Revoked == null &&        
                             t.Expires > utcNow));
        }


       
        public async Task<UsuarioPortal> GetIdUsuario(int id)
        {
            return await _repositoryContext.UsuarioPortals
                         .Include(u => u.rol)
                         .Include(u => u.RefreshTokens)
                         .FirstOrDefaultAsync(u => u.id == id);
        }



        public async Task<UsuarioPortal> GetByEmail(string Email)
        {
            return await _repositoryContext.UsuarioPortals
                         .Include(u => u.rol)
                         .Include(u => u.RefreshTokens)
                         .FirstOrDefaultAsync(u => u.correo.ToLower() == Email.ToLower());
        }



    }


    public class AuthenticationException : Exception
    {
        public AuthenticationException(string message) : base(message)
        {
        }
    }



}
