using Contracts;
using Services.Generic;
using Entities.DbModels;
using Persistence;
using Entities.EPModels;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Services
{
    public class UsuarioSecomLabService : RepositoryBase<UsuarioSecomLab>, IUsuarioSecomLabService
    {
        private readonly AppSettings _appSettings;
        public UsuarioSecomLabService(RepositoryContext repositoryContext, IOptions<AppSettings> appSettings) : base(repositoryContext)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponseSecomlab Authenticate(AuthenticateRequestSecomlab model)
        {
            var user = findByCondition(x => x.user == model.Username && x.pass== model.Password).ToList().FirstOrDefault();

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponseSecomlab(user, token);
        }

        private string generateJwtToken(UsuarioSecomLab user)
        {
            // generate token that is valid for 7 days
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

    }
}
