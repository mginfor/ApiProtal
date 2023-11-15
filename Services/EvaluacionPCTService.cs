using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class EvaluacionPCTService : RepositoryBase<EvaluacionPCT>, IEvaluacionPCTService
    {
        public EvaluacionPCTService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }

        public bool addDocumentoInBrecha(int idBrecha, int idDocumento)
        {
            var brecha = this.find(idBrecha);
            brecha.idDocumento = idDocumento;
            this.update(brecha);
            return true;
        }

        public List<EvaluacionPCT> getBrechasByIdEvaluacion(int idEvaluacion)
        {
            return this.findByCondition(x => x.idEvaluacion == idEvaluacion && x.brecha && x.anulado == false, "documento").ToList();
        }









    }
}
