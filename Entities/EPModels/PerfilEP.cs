using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class PerfilEP
    {
        public int? id { get; set; }
        public string codigoPerfil { get; set; }
        public string descripcionPerfil { get; set; }
        public int? codigoSector { get; set; }
        public int? codigoSubSector { get; set; }
        public DateTime? fechaVigencia { get; set; }
        public int? idGrupoPerfil { get; set; }
        public int? flgVigencia { get; set; }
        public int? flgChileValora { get; set; }
        public int? flgHomologado { get; set; }
    }
}
