using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tges_eval_ejd")]
    public class EvaluacionEJD
    {
        [Key]
        [Column("CRR_IDEVALEJD")]
        public int id { get; set; }

        [Column("CRR_INSTRUMENTO")]
        public int idInstrumento { get; set; }

        [Column("CRR_DETINSTRUMENTO")]
        public int idDetInstrumento { get; set; }

        [Column("CRR_EVALUACION")]
        public int idEvaluacion { get; set; }

        [Column("CRR_ACTIVIDAD")]
        public int idActividad { get; set; }

        [Column("CRR_CRITERIO")]
        public int idCriterio { get; set; }

        [Column("NRO_PREGUNTA")]
        public int nroPregunta { get; set; }

        [Column("ITEM")]
        public int? item { get; set; }

        [Column("NOTA")]
        public double? nota { get; set; }

        [Column("FLG_BRECHA")]
        public bool brecha { get; set; }

        [Column("FLG_CRITICO")]
        public bool critico { get; set; }

        [Column("GLS_BRECHA")]
        public string? glsBrecha { get; set; }

        [Column("GLS_RECOMENDACION")]
        public string? glsRecomendacion { get; set; }

        [Column("FLG_ESTADO_PRUEBA")]
        public bool estadoPrueba { get; set; }

        [Column("ID_USUARIO")]
        public int? idUsuario { get; set; }
    }
}
