using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class EvaluacionExportarService : RepositoryBase<Evaluacion>, IEvaluacionExportarService
    {
        public EvaluacionExportarService(RepositoryContext repositoryContext) : base(repositoryContext)
        {

        }

        public List<EvaluacionExportarEP> getAllEvaluationsByCliente(int idCliente)
        {
            var list = this.findByCondition(x => x.idCliente == idCliente).ToList();
            //var list = this.findByCondition(x => x.idCliente == 0).ToList();
            List<EvaluacionExportarEP> evalList = new List<EvaluacionExportarEP>();
            foreach (var eval in list)
            {
                evalList.Add(new EvaluacionExportarEP
                {
                    idCliente = eval.idCliente,
                    idFaena = eval.idFaena,
                    idPerfil = eval.idPerfil
                });
            }
            return evalList;
        }

        public List<EvaluacionExportarEP> getAllEvaluationsByClienteByPerfil(int idCliente, int idPerfil)
        {
            var list = this.findByCondition(x => x.idCliente == idCliente && x.idPerfil == idPerfil).ToList();

            //var list = this.findByCondition(x => x.idCliente == 0).ToList();
            List<EvaluacionExportarEP> evalList = new List<EvaluacionExportarEP>();
            foreach (var eval in list)
            {
                evalList.Add(new EvaluacionExportarEP
                {
                    idCliente = eval.idCliente,
                    idFaena = eval.idFaena,
                    idPerfil = eval.idPerfil

                });
            }
            return evalList;
        }
    }
}
