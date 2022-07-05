using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    [Table("tges_eval_prot")]
    public class EvaluacionPROT
    {
        [Key]
        [Column("CRR_IDEVALPROT")]
        public int id { get; set; }

        [Column("CRR_INSTRUMENTO")]
        public int idInstrumento{ get; set; }

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

        [Column("CATEGORIA")]
        public string? categoria { get; set; }

        [Column("SUPRA_CATEGORIA")]
        public int? supraCategoria{ get; set; }

        [Column("ITEM")]
        public int? item { get; set; }

        [Column("NOTA_1")]
        public double? nota1 { get; set; }

        [Column("FLG_BRECHA")]
        public bool brecha{ get; set; }

        [Column("FLG_CRITICO")]
        public bool critico{ get; set; }

        [Column("GLS_BRECHA")]
        public string? glsBrecha { get; set; }

        [Column("GLS_RECOMENDACION")]
        public string? glsRecomendacion{ get; set; }

        [Column("FLG_ESTADO_PRUEBA")]
        public bool estadoPrueba { get; set; }

        [Column("ID_USUARIO")]
        public int? idUsuario{ get; set; }

        [Column("FLG_ANULADO")]
        public bool anulado { get; set; }

        [Column("CRR_DOCUMENTOBRECHA")]
        public int? idDocumento { get; set; }

        //FK
        [ForeignKey("idDocumento")]
        public DocumentoBrecha documento { get; set; }
    }
}
