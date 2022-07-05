using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class EvaluacionEIService : RepositoryBase<EvaluacionEI>, IEvaluacionEIService
    {
        public EvaluacionEIService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }
        public bool addDocumentoInBrecha(int idBrecha, int idDocumento)
        {
            var brecha = this.find(idBrecha);
            brecha.idDocumento = idDocumento;
            this.update(brecha);
            return true;
        }

        public List<EvaluacionEI> getBrechasByIdEvaluacion(int idEvaluacion)
        {
            return this.findByCondition(x => x.idEvaluacion == idEvaluacion && x.brecha, "documento").ToList();
        }
    }
}
