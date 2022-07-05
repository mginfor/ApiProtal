using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;


namespace Services
{
    public class PerfilService : RepositoryBase<Perfil>, IPerfilService
    {
        public PerfilService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
        public List<Perfil> getAllPerfiles()
        {
            return this.findByCondition(x => x.flgVigencia != 0).ToList();
        }
    }
}
