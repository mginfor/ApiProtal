using Entities.DbModels;
using Entities.EPModels;
using System.Collections.Generic;
namespace Contracts
{
    public interface IExportarService
    {
        public List<Exportar> getDatosExportacion(int idPerfil, int idFaena, int idCliente);
        public List<BrechasCandidato> getProcesoBrecha(int idEvaluacion);



    }
}
