using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using Persistence;
using Services.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class CriterioServices: RepositoryBase<EvaluacionPCT>, ICriterioService
    {

        private RepositoryContext _dbContext;

        public CriterioServices(RepositoryContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public List<CriterioDto> GetCriteriosByTgesEvalPct (int tgesEvalPctId)
        {
            var criterios = (from tep in _dbContext.EvaluacionPCT
                             join criterio in _dbContext.Criterio on tep.idCriterio equals criterio.IdCriterio
                             where tep.idEvaluacion == tgesEvalPctId && tep.brecha
                             select new CriterioDto
                             {
                                 CrrIdCriterio = criterio.IdCriterio,
                                 GlsCriterio = criterio.DescripcionCriterio

                             }).Distinct().ToList();

            return criterios;
        }
    }
}
