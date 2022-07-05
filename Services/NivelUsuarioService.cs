using Contracts;
using Services.Generic;
using Entities.DbModels;
using Persistence;
using System.Linq;

namespace Services
{
    public class NivelUsuarioService : RepositoryBase<NivelUsuario>, INivelUsuarioService
    {
        public NivelUsuarioService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
    }
}
