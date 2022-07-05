using Contracts;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class CandidatoService : RepositoryBase<Candidato>, ICandidatoService
    {
        private RepositoryContext db;

        public CandidatoService(RepositoryContext repositoryContext) : base(repositoryContext)
        {
            db = new RepositoryContext();

        }

        public Candidato getCandidato(int idCandidato, string pass)
        {
            return this.findByCondition(x => x.id == idCandidato)
                .Where(x => x.passBrecha == pass)
                .FirstOrDefault();
        }


    }
}
