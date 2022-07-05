using Contracts.Generic;
using Entities.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IDocumentoBrechaService:IRepositoryBase<DocumentoBrecha>
    {
        public DocumentoBrecha saveDocumento(DocumentoBrecha documento);
        public List<DocumentoBrecha> getAllDocumentosByIdEvaluacion(int idEvaluacion);
    }
}
