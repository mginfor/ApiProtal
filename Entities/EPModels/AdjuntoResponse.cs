using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class AdjuntoResponse
    {
        public string NombreArchivo { get; set; }
        public string ContentType { get; set; }
        public string Base64 { get; set; }

        public AdjuntoResponse(string nombreArchivo, string contentType, string base64)
        {
            NombreArchivo = nombreArchivo;
            ContentType = contentType;
            Base64 = base64;
        }
    }
}
