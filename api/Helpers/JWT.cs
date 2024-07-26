using Microsoft.Extensions.Configuration;

namespace api.Helpers
{
    public class JWT
    {
        private readonly IConfiguration _configuration;
        public JWT(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Key { get { return _configuration["JWT:Key"].ToString(); } }
        public string Issuer { get { return _configuration["JWT:Issuer"].ToString(); } }
        public string Audience { get { return _configuration["JWT:Audience"].ToString(); } }
        public double DurationInMinutes { get { return double.Parse(_configuration["JWT:DurationInMinutes"].ToString()); } }
    }
}
