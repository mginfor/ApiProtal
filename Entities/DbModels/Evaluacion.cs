using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Entities.DbModels
{
    [Table("tges_evaluacion")]
    public class Evaluacion
    {
        [Key]
        [Column("CRR_IDEVALUACION")]
        public int? id { get; set; }
        
        [Column("CRR_UCL")]
        public int? idUcl { get; set; }
        
        [Column("COD_TIPO_PROCESO")]
        public string tipoProceso { get; set; }
        
        [Column("CRR_PERFIL")]
        public int? idPerfil { get; set; }
        
        [Column("CRR_EQUIPOMODELO")]
        public int? idEquipoModelo { get; set; }
        
        [Column("CRR_CANDIDATO")]
        public int? idCandidato { get; set; }
        
        [Column("RUN_CANDIDATO")]
        public int? runCandidato { get; set; }
        
        [Column("CRR_CLIENTE")]
        public int? idCliente { get; set; }
        
        [Column("RUN_CLIENTE")]
        public int? runCliente { get; set; }
        
        [Column("RUN_EVALUADOR")]
        public int? runEvaluador { get; set; }
        
        [Column("FECHA_PCT")]
        public DateTime? fechaPCT { get; set; }
        
        [Column("FECHA_EJD")]
        public DateTime? fechaEJD{ get; set; }
        
        [Column("FECHA_EI")]
        public DateTime? fechaEI{ get; set; }
        
        [Column("FECHA_PROT1")]
        public DateTime? fechaProt1{ get; set; }
        
        [Column("FECHA_PROT2")]
        public DateTime? fechaProt2 { get; set; }
        
        [Column("FECHA_INFORME")]
        public DateTime? fechaInforme { get; set; }
        
        [Column("CRR_FAENA")]
        public int? idFaena { get; set; }
        
        [Column("NOTA_PCT")]
        public double? notaPCT { get; set; }
        
        [Column("NOTA_PROT")]
        public double? notaPROT { get; set; }
        
        [Column("NOTA_EI")]
        public double? notaEI { get; set; }
        
        [Column("NOTA_EJD")]
        public double? notaEJD { get; set; }
        
        [Column("NOTA_PROT2")]
        public double? notaPROT2 { get; set; }
        
        [Column("PORC_PCT")]
        public double? porcPCT { get; set; }
        
        [Column("PORC_PROT")]
        public double? porcPROT { get; set; }
        
        [Column("PORC_EJD")]
        public double? porcEJD { get; set; }
        
        [Column("PORC_EI")]
        public double? porcEI { get; set; }
        
        [Column("PORC_PROT2")]
        public double? porcPROT2 { get; set; }
        
        [Column("PORC_CUMPLIMIENTO")]
        public double? porcCumplimiento { get; set; }
        
        [Column("RESULTADO_GENERAL")]
        public string resultadoGeneral { get; set; }
        
        [Column("COD_ESTADO_PROCESO")]
        public int? estadoProceso { get; set; }
        
        [Column("COD_RESULTADO")]
        public int? resultado { get; set; }
        
        [Column("FLG_CIERRE_PROCESO")]
        public bool cierreProceso { get; set; }
        
        [Column("FLG_GENERA_INFORME")]
        public int? generaInforme { get; set; }
        
        [Column("FECHA_INGRESO_PROCESO")]
        public DateTime? fechaIngresoProceso { get; set; }
        
        [Column("CRR_IDINFORME")]
        public int? idInforme { get; set; }
        
        [Column("DIGITO_INFORME")]
        public string digitoInforme { get; set; }
        
        [Column("PORC_MIN_APROBACION")]
        public int? porcMinimoAprobacion { get; set; }
        
        [Column("ID_USUARIO_CT")]
        public int? idUsuarioCT { get; set; }
        
        [Column("ID_USUARIO")]
        public int? idUsuario { get; set; }
        
        [Column("ID_USUARIO_BRECHA")]
        public int? idUsuarioBrecha { get; set; }
        
        [Column("FLG_RESP_BRECHA")]
        public int? respBrecha { get; set; }
        
        [Column("FLG_FIRMA")]
        public int? firma { get; set; }
        
        [Column("NOMBRE_RESP_BRECHA")]
        public string nombreRespBrecha { get; set; }
        
        [Column("MESES_VIGENCIA")]
        public int? mesesVigencia { get; set; }
        
        [Column("CRR_CONTEXTO")]
        public int? idContexto { get; set; }
        
        [Column("CRR_LUGAR")]
        public int? idLugar { get; set; }
        
        [Column("GLS_DIRECC_PROT")]
        public string glsDireccPROT { get; set; }
        
        [Column("GLS_CONTRATO")]
        public string glsContrato { get; set; }
        
        [Column("FLG_PROCESO_ANULADO")]
        public int? procesoAnulado { get; set; }
        
        [Column("FLG_CIERRE_ADM")]
        public int? cierreAdm { get; set; }
        
        [Column("CORRE_REAC_ANTERIOR")]
        public int? correReacAnterior { get; set; }
        
        [Column("CRR_PLANCANDIDATO")]
        public int? idPlancandidato { get; set; }
        
        [Column("CRR_PLANIFICACION")]
        public int? idPlanificacion { get; set; }
        
        [Column("GLS_PROCESO_ANULADO")]
        public string glsProcesoAnulado { get; set; }
        
        [Column("FLG_PROCESO_ANTIGUO")]
        public int? procesoAntiguo { get; set; }
        
        [Column("FECHA_ULTIMOINFORME")]
        public DateTime? fechaUltimoInforme { get; set; }
        
        [Column("CRR_CONTRATO")]
        public int? idContrato { get; set; }
        
        [Column("FLG_ERROR_NOTA")]
        public int? errorNota { get; set; }

        [Column("FLG_SINMODELO")]
        public int? sinModelo { get; set; }
        
        [Column("CRR_PROYECTO_CONTRATO")]
        public int? idProyectoContrato { get; set; }

        [Column("FECHA_VIGENCIAINFORME")]
        public DateTime? fechaVigenciaInforme { get; set; }
    }

}
