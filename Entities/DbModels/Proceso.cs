using System;

namespace Entities.DbModels
{
    public class Proceso
    {
        public string tipoProceso { get; set; }
        public int idCandidato { get; set; }
        public string estado { get; set; }
        public int idEstado { get; set; }
        public string runCandidato { get; set; }
        public string dni_Pasaporte { get; set; }
        public string nombreCandidato { get; set; }
        public DateTime? fechaInforme { get; set; }
        public DateTime? fechaPct { get; set; }
        public DateTime? fechaProt { get; set; }
        public double? porcPct { get; set; }
        public double? porcProt { get; set; }
        public double? porcCumplimiento { get; set; }
        public int? idResultado { get; set; }
        public string descResultado { get; set; }
        public int? idInforme { get; set; }
        public bool? conFirma { get; set; }
        public int rutCliente { get; set; }
        public string nombreCliente { get; set; }
        public int? mesesVigencia { get; set; }
        public int idCliente { get; set; }
        public string equipo { get; set; }
        public string modelo { get; set; }
        public string perfil { get; set; }
        public int? idEvaluacion { get; set; }
        public string zonaFaena { get; set; }
        public string nombreFaena { get; set; }
        public bool conProt { get; set; }
        public bool  conPct { get; set; }
        public string vigencia { get; set; }
        public int brechaPct { get; set; }
        public int brechaProt { get; set; }
        public int brechaEI { get; set; }
        public string run_md5{ get; set; }
        public DateTime? fechaVigenciaInforme { get; set; }
        public DateTime? fechaProt2 { get; set; }
        public double? porcProt2 { get; set; }
        public DateTime? fechaEi { get; set; }
        public double? porcEi { get; set; }
        public string resultadoGeneral { get; set; }
        public int? codelcoVp { get; set; }

          
    }
}
