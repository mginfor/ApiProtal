using Entities.DbModels;

namespace Entities.EPModels
{
    public class AuthenticateResponseSecomlab
    {
        public UsuarioSecomLab usuario { get; set; }
        public string Token { get; set; }
        public int Expira { get; set; }

        public AuthenticateResponseSecomlab(UsuarioSecomLab user, string token)
        {
            usuario = user;
            Token = token;
            Expira = 8;
        }
    }
}
