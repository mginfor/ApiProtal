using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System.Linq;

namespace api.Helpers
{
    public class AutorizacionHelper
    {
        private readonly RepositoryContext _repositoryContext;
        public AutorizacionHelper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public AutorizacionHelper()
        {
            var repositoryContext = new RepositoryContext(); 
            var autorizacionHelper = new AutorizacionHelper(repositoryContext);

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
    }
    public static class EnumPermisos
    {
        public const string GestionInformes = "GestionInformes";
        public const string TableroGestion = "TableroGestion";
        public const string TratamientoBrecha = "TratamientoBrecha";
        public const string Descargas = "Descargas";
    }
}
