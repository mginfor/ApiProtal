using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tcnf_det_instrumento")]
    public class DetInstrumentos
    {
        [Key]
        [Column("CRR_IDDETINSTRUMENTO")]
        public int id { get; set; }

        [Column("CRR_INSTRUMENTO")]
        public int idInstrumento { get; set; }

        [Column("CRR_ACTIVIDAD")]
        public int Actividad { get; set; }

        [Column("CRR_CRITERIO")]
        public int Criterio { get; set; }

        [Column("NRO_PREGUNTA")]
        public int NPregunta { get; set; }

        [Column("CATEGORIA")]
        public string Categoria { get; set; }

        [Column("ITEM")]
        public int Item { get; set; }

        [Column("FLG_CRITICO")]
        public bool Critico { get; set; }

        [Column("DESC_PREGUNTA")]
        public string Pregunta { get; set; }

        [Column("DESC_RESPUESTA")]
        public string Respuesta { get; set; }

        [Column("GLS_BRECHA")]
        public string Brecha { get; set; }

        [Column("GLS_RECOMENDACION")]
        public string Recomendaciones { get; set; }

        [Column("NOTA_REF")]
        public decimal Nota { get; set; }

        [Column("ALTERNATIVA_CORRECTA")]
        public string AlternativaCorrecta { get; set; }

        [Column("COD_CRITERIO_HMLGO")]
        public string CodCriterio { get; set; }

        [Column("FLG_PCTCERTIFICACION")]
        public bool FlgPctCertificacion { get; set; }

        [Column("CRR_IDPOOLPREGUNTA")]
        public int IdPoolPregunta { get; set; }

        [Column("LLAVE_PREGUNTA")]
        public string LLavePregunta { get; set; }

        [Column("FLG_COMP_COND_CRIT")]
        public bool FlgCompetenciaCritica { get; set; }

        [Column("CRR_COMPETENCIA_CONDICIONANTE")]
        public int CompentenciaCondicionante { get; set; }

        [Column("LLAVE_COMPETENCIA_CONDICIONANTE")]
        public string LLaveCompetenciaCondicionante { get; set; }
    }
}
