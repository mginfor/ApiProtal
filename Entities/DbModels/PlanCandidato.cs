using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DbModels
{
    [Table("tges_plan_candidato")]
    public class PlanCandidato
    {
        [Key]
        [Column("CRR_IDPLANCANDIDATO")]
        public int id { get; set; }

        [Column("CRR_PLANIFICACION")]
        public int? CrrPlanificacion { get; set; }

        [Column("CRR_CANDIDATO")]
        public int? CrrCandidato { get; set; }

        [Column("CRR_ESTADO_PROCESO")]
        public int? CrrEstadoProceso { get; set; }

        [Column("TIPO_PROCESO")]
        public string TipoProceso { get; set; }

        [Column("CRR_PERFIL")]
        public int? CrrPerfil { get; set; }

        [Column("CRR_UCL")]
        public int? CrrUcl { get; set; }

        [Column("CRR_EQUIPO")]
        public int? CrrEquipo { get; set; }

        [Column("FLG_ANULADO")]
        public bool FlgAnulado { get; set; }

        [Column("CORRE_REAC_ANTERIOR")]
        public int? CorreReacAnterior { get; set; }

        [Column("FLG_SISMICIDAD")]
        public bool FlgSismicidad { get; set; }

        [Column("FECHA_CREACION")]
        public DateTime FechaCreacion { get; set; }

        [Column("CRR_EVALUACION")]
        public int CrrEvaluacion { get; set; }

        [Column("FLG_PROT")]
        public bool FlgProt { get; set; }

        [Column("FLG_PCT")]
        public bool FlgPct { get; set; }

        [Column("FLG_EI")]
        public bool FlgEi { get; set; }

        [Column("CRR_CONTRATO")]
        public int? CrrContrato { get; set; }

        [Column("FECHA_MODIFICACION")]
        public DateTime? FechaModificacion { get; set; }

        [Column("USUARIO_MODIFICACION")]
        public int? UsuarioModificacion { get; set; }

        [Column("ESTADO_COBRO")]
        public int EstadoCobro { get; set; }

        [Column("FLG_PROT2")]
        public bool FlgProt2 { get; set; }

        [Column("FLG_EJD")]
        public bool FlgEjd { get; set; }

        [Column("CRR_PROYECTO_CONTRATO")]
        public int? CrrProyectoContrato { get; set; }

        [Column("FLG_CODELCOVP")]
        public bool FlgCodelcoVp { get; set; }

        [Column("FLG_MARCA_ESTADISTICA")]
        public bool FlgMarcaEstadistica { get; set; }

        [Column("FLG_SOCIALIZABLE")]
        public bool FlgSocializable { get; set; }

        [Column("FLG_ELEGIBILIDAD")]
        public bool FlgElegibilidad { get; set; }

        [Column("CRR_RUBRICA")]
        public int? CrrRubrica { get; set; }

        [Column("AGRUPADOR")]
        public string Agrupador { get; set; }

        // Relaciones con otras entidades
      

        [ForeignKey("CrrCandidato")]
        public virtual Candidato Candidato { get; set; }

        [ForeignKey("CrrEstadoProceso")]
        public virtual EstadoProceso EstadoProceso { get; set; }

        [ForeignKey("CrrPerfil")]
        public virtual Perfil Perfil { get; set; }

       

    }
}
