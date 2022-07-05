using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tges_documento_brecha")]
    public class DocumentoBrecha
    {
        [Key]
        [Column("CRR_IDDOCUMENTOBRECHA")]
        public int id { get; set; }

        [Column("CRR_EVALUACION")]
        public int idEvaluacion { get; set; }
        
        [Column("DESC_NOMBREDOCUMENTO")]
        public string nombreDocumento { get; set; }
        
        [Column("DESC_MEDIOTRATAMIENTO")]
        public string medioTratamiento { get; set; }
        
        [Column("GLS_NOMBREENTIDAD")]
        public string entidadTratamiento { get; set; }
        
        [Column("RUTADOCUMENTO")]
        public string ruta { get; set; }
    }
}
