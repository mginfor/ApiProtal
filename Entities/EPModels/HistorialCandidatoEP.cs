using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.EPModels
{
    public class HistorialCandidatoEP
    {
        public string nombreCandidato { get; set; }
        public string run { get; set; }
        public string dni_Pasaporte { get; set; }   
        public List<HistoricoProcesoEP> procesos { get; set; }

        public int idCandidato { get; set; }


        public HistorialCandidatoEP()
        {
            this.nombreCandidato = "";
            this.run = "";
            this.dni_Pasaporte = "";
            procesos = new();
            this.idCandidato = 0;
        }
        public HistorialCandidatoEP(string nombreCandidato, string run, string dniPasaporte, List<HistoricoProcesoEP> data, int id = 0)
        {
            this.nombreCandidato = nombreCandidato;
            this.run = run;
            this.dni_Pasaporte = dniPasaporte;
            this.procesos = data;
            this.idCandidato = id;
        }




    }

    public class HistoricoProcesoEP
    {
        
        public int? idEvaluacion { get; set; }
        public string NombreEmpresa { get; set; }
        public string Faena { get; set; }
        public string Perfil { get; set; }
        public string FechaInforme { get; set; }
        public string Equipo { get; set; }
        public string Resultado { get; set; }

        

     


        public HistoricoProcesoEP(int? idEvaluacion, string nombreEmpresa, string perfil, string FechaInforme, string equipo, string resultado, string faena)
        {
           
            this.idEvaluacion = idEvaluacion;
            this.NombreEmpresa = nombreEmpresa;
            this.Perfil = perfil;
            this.FechaInforme = FechaInforme;
            this.Equipo = equipo;
            this.Resultado = resultado;
            this.Faena = faena;
            

        }
        public HistoricoProcesoEP()
        {
           
            this.idEvaluacion = 0;
            this.NombreEmpresa = "";
            this.Perfil = "";
            this.FechaInforme = "";
            this.Equipo = "";
            this.Resultado = "";
            this.Faena = "";
           
        }



    }






}
