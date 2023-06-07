
using System.Collections.Generic;
using System.Linq;
using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using Microsoft.EntityFrameworkCore;

namespace Services
{
    public class PermisosServices: RepositoryBase<Permisos>, IPermisosServices
    {

        private RepositoryContext db;

        public PermisosServices(RepositoryContext repositoryContext) : base(repositoryContext)
        {

            db = new RepositoryContext();

        }

        public List<Permisos> GetAllPermisos()
        {
             return this.findAll().ToList();
        }

        public Permisos GetPermisosByid(int id)
        {

            return this.findByCondition(x => x.id == id).ToList().FirstOrDefault();


        }

        public Permisos getCandidato(int idPermiso, string NombrePermiso)
        {
            return this.findByCondition(x => x.id == idPermiso)
                .Where(x => x.NombrePermiso == NombrePermiso)
                .FirstOrDefault();
        }

    }
}
