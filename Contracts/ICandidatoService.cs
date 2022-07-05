using Contracts.Generic;
using Entities.DbModels;


namespace Contracts
{
    public interface ICandidatoService : IRepositoryBase<Candidato>
    {
        public Candidato getCandidato(int idCandidato, string pass);
    }
}
