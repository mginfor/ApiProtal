using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.DbModels
{
    public class CriterioGrafico
    {
        public int cantidadM { get; set; }
        public int cantidadB { get; set; }
        public string codActividad { get; set; }
        public string codCriterio { get; set; }
        public string faena { get; set; }
    }
    public class Empresas
    {
        public int codCliente { get; set; }
        public string nombreCliente { get; set; }
    }
    public class FaenaPerfilCandidato
    {
        public string faena { get; set; }
        public string codPerfil { get; set; }
        public string perfil { get; set; }
        public string run { get; set; }
        public string candidato { get; set; }
        public string nombreCliente { get; set; }
    }
    public class GraficoGeneral
    {
        public int cantidad { get; set; }
        public double porcentajePerfil { get; set; }
        public string perfil { get; set; }
        public string codPerfil { get; set; }
        public string faena { get; set; }
        public double porcentajeCumplimiento { get; set; }
    }
    public class LogroInstrumentos
    {
        public int totalGeneral { get; set; }
        public int PCT_insuficiente { get; set; }
        public int PCT_Suficiente { get; set; }
        public int PCT_Superior { get; set; }
        public int PROT_Suficiente { get; set; }
        public int PROT_Superior { get; set; }
        public int EI_insuficiente { get; set; }
        public int EI_Suficiente { get; set; }
        public int EI_Superior { get; set; }
    }
    public class PerfilBrechas
    {
        public int Cantidad { get; set; }
        public string Codigo { get; set; }
        public string Perfil { get; set; }
        public string Brecha { get; set; }
        public string Recomendacion { get; set; }
    }
    public class PersonasResultado
    {
        public int cantidadResultado { get; set; }
        public string tipoResultado { get; set; }
        public int referencia { get; set; }
    }
    public class ProcesoLogroCandidato
    {
        public double cumplimientoPersona { get; set; }
        public double cumplimientoIndustria { get; set; }
        public double perfilMinimo { get; set; }
        public double perfilMaximo { get; set; }
        public double cumplimientoPCT { get; set; }
        public double cumplimientoPROT { get; set; }
        public double cumplimientoEI { get; set; }
    }
    public class ProcesosLogros
    {
        public int vigentes { get; set; }
        public int noVigentes { get; set; }
        public double cumplimientoEmpresa { get; set; }
        public double cumplimientoIndustria { get; set; }
        public string cliente { get; set; }
    }
}
