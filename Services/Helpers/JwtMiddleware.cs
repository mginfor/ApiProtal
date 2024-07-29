using Contracts;
using Entities.EPModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace api.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }


        #region Secomlab
        public async Task Invoke(HttpContext context, IUsuarioSecomLabService userSecomLabService, IUsuarioPortalService? userPortalService = null)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                attachUserToContext(context, token, userSecomLabService, userPortalService);

            }

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, string token, IUsuarioSecomLabService userService = null, IUsuarioPortalService? userPortalService = null)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // Obtener los permisos del token
                var userPermissions = jwtToken.Claims.Where(c => c.Type == "permission").Select(c => c.Value).ToList();
                var userRoles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                context.Items["User"] = userService.find(userId);
                context.Items["Permissions"] = userPermissions;
                context.Items["Roles"] = userRoles;
                if (userPortalService != null)
                {
                    context.Items["User"] = userPortalService.find(userId);
                }
            }
            catch
            {
                // do nothing if jwt validation fails
            }
        }






        #endregion

    }




}
