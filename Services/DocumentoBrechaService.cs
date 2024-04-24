using Contracts;
using Entities.DbModels;
using Persistence;
using Services.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class DocumentoBrechaService : RepositoryBase<DocumentoBrecha>, IDocumentoBrechaService
    {
        public DocumentoBrechaService(RepositoryContext repositoryContext) : base(repositoryContext)
        { }

        public List<DocumentoBrecha> getAllDocumentosByIdEvaluacion(int idEvaluacion)
        {
            return this.findByCondition(x => x.idEvaluacion == idEvaluacion).ToList();
        }

        public DocumentoBrecha saveDocumento(DocumentoBrecha documento)
        {
            this.create(documento);
            return this.findByCondition(x => x.nombreDocumento == documento.nombreDocumento).OrderByDescending(x => x.id)
                .ToList().FirstOrDefault();
        }

    }
}
