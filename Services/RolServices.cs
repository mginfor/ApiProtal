using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class RolServices: RepositoryBase<Rol>, IRolService

    {
        private RepositoryContext db;
        public RolServices(RepositoryContext repositoryContext) : base(repositoryContext)
        {

           db = new RepositoryContext();

        }

        public List<Rol> GetAllRol()
        {
            return this.findAll().ToList();
        }

        public Rol GetRolByid(int id)
        {

            return this.findByCondition(x => x.id == id).ToList().FirstOrDefault();


        }

         public Rol getCandidato(int idRol, string NombreRol)
         {
            return this.findByCondition(x => x.id == idRol)
                .Where(x => x.NombreRol == NombreRol)
                .FirstOrDefault();
         }

    




    }
}      
