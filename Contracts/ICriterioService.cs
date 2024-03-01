using Contracts.Generic;
using Entities.DbModels;
using Entities.EPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface ICriterioService: IRepositoryBase<EvaluacionPCT>
    {
        List<CriterioDto> GetCriteriosByTgesEvalPct(int tgesEvalPctId);
    }
}
