using Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities.DbModels;
using System.Threading.Tasks;
using Entities.EPModels;
using System.Diagnostics;

namespace Services
{
    public class ProcesoService : IProcesoService
    {

        private RepositoryContext db;
        public ProcesoService()
        {
            db = new RepositoryContext();
        }

        public List<ProcesoInforme> getProcesosByIdInforme(int idInforme)
        {
            var query = "select * from vw_portal_proceso_Informe  where idInforme = {0};";

            return db.ProcesoPortalInforme
                .FromSqlRaw(query, idInforme)
                .ToList();
        }

        public List<Proceso> getProcesosByIdEvaluacion(int idEvaluacion)
        {
            var query = "select * from vw_portal_proceso where idEvaluacion = {0};";

            return db.ProcesoPortal
                .FromSqlRaw(query, idEvaluacion)
                .ToList();
        }
        public List<Proceso> getProcesosByHash(string hash)
        {
            var query = "select * from vw_portal_proceso where idResultado=1 and run_md5 = {0};";

            return db.ProcesoPortal
                .FromSqlRaw(query, hash)
                .ToList();
        }



        public List<Proceso> getProcesosByProcesoEP(ProcesoEP proceso)
        {
            var query = "select * from vw_portal_proceso where idCliente = {0} order by fechaInforme desc ;";
            var resultado = db.ProcesoPortal
                .FromSqlRaw(query, Convert.ToInt32(proceso.idCliente)).ToList();


            if (!string.IsNullOrEmpty(proceso.zonaFaena))
            {
                resultado = resultado.Where(x => x.zonaFaena == proceso.zonaFaena).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.runCandidato))
            {
                resultado = resultado.Where(x => x.runCandidato == proceso.runCandidato).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.dni_Pasaporte))
            {
                resultado = resultado.Where(x => x.dni_Pasaporte == proceso.dni_Pasaporte).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.estado))
            {
                resultado = resultado.Where(x => x.estado == proceso.estado).ToList();
            }
            //revisar
            if (!string.IsNullOrEmpty(proceso.resultado))
            {
                resultado = resultado.Where(x => x.idResultado == Convert.ToInt32(proceso.resultado)).ToList();
            }
            //revisar
            if (!string.IsNullOrEmpty(proceso.vigencia))
            {
                resultado = resultado.Where(x => x.vigencia == proceso.vigencia).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.fechaInicio))
            {
                resultado = resultado.Where(x => x.fechaInforme >= DateTime.Parse(proceso.fechaInicio)).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.fechaFinal))
            {
                resultado = resultado.Where(x => x.fechaInforme <= DateTime.Parse(proceso.fechaFinal)).ToList();
            }

            return resultado.ToList();

        }


        public List<Proceso> getProcesosByProcesoTratamiento(ProcesoEP proceso)
        {
            var query = "select * from vw_portal_proceso_Tratamineto where idCliente = {0} order by fechaInforme desc ;";
            var resultado = db.ProcesoPortal
                .FromSqlRaw(query, Convert.ToInt32(proceso.idCliente)).ToList();


            if (!string.IsNullOrEmpty(proceso.zonaFaena))
            {
                resultado = resultado.Where(x => x.zonaFaena == proceso.zonaFaena).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.runCandidato))
            {
                resultado = resultado.Where(x => x.runCandidato == proceso.runCandidato).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.dni_Pasaporte))
            {
                resultado = resultado.Where(x => x.dni_Pasaporte == proceso.dni_Pasaporte).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.estado))
            {
                resultado = resultado.Where(x => x.estado == proceso.estado).ToList();
            }
            //revisar
            if (!string.IsNullOrEmpty(proceso.resultado))
            {
                resultado = resultado.Where(x => x.idResultado == Convert.ToInt32(proceso.resultado)).ToList();
            }
            //revisar
            if (!string.IsNullOrEmpty(proceso.vigencia))
            {
                resultado = resultado.Where(x => x.vigencia == proceso.vigencia).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.fechaInicio))
            {
                resultado = resultado.Where(x => x.fechaInforme >= DateTime.Parse(proceso.fechaInicio)).ToList();
            }
            if (!string.IsNullOrEmpty(proceso.fechaFinal))
            {
                resultado = resultado.Where(x => x.fechaInforme <= DateTime.Parse(proceso.fechaFinal)).ToList();
            }

            return resultado.ToList();

        }

        public List<Proceso> getProcesosByIdEvaluacionTratamiento(int idEvaluacion)
        {
            var query = "select * from vw_portal_proceso_Tratamineto where idEvaluacion = {0};";

            return db.ProcesoPortal
                .FromSqlRaw(query, idEvaluacion)
                .ToList();
        }



        public List<BrechaPortal> getbrechasByHash(int idEvaluacion)
        {
            var query = "SELECT distinct " +
                        "pp.GLS_BRECHA AS BRECHA, " +
                        "if (epct.CRR_DOCUMENTOBRECHA > 0, 'SI', 'NO') as Brecha_Tratada, " +
                        "tp.DESC_PERFIL as Prefil_Brecha " +
                        "FROM tges_evaluacion ev, tges_eval_pct epct, tcnf_pool_preguntas pp, tcnf_det_instrumento di, tg_perfil tp " +
                        "WHERE ev.CRR_IDEVALUACION = epct.CRR_EVALUACION " +
                        "AND ev.FLG_PROCESO_ANULADO = 0 " +
                        "AND ev.FLG_CIERRE_ADM = 0 " +
                        "AND ev.COD_ESTADO_PROCESO <> 0 " +
                        "AND epct.FLG_ANULADO = 0 " +
                        "AND epct.FLG_BRECHA <> 0 " +
                        "AND ev.FECHA_VIGENCIAINFORME >= NOW() " +
                        "AND ev.CRR_IDEVALUACION = {0} " +
                        "and ev.CRR_PERFIL = tp.CRR_IDPERFIL " +
                        "AND di.CRR_IDDETINSTRUMENTO = epct.CRR_DETINSTRUMENTO " +
                        "AND pp.CRR_IDPOOLPREGUNTA = di.CRR_IDPOOLPREGUNTA " +
                        "ORDER BY 1;";

            return db.BrechaPortal
                .FromSqlRaw(query, idEvaluacion)
                .ToList();
        }

        public List<ProcesoReportabilidad> GetProcesosReportabilidad()
        {
            var query = "select * from vw_reportabilidad;";
            var resultado = db.ProcesoReportabilidadPortal
                .FromSqlRaw(query).ToList();
       
            return CalcularPorcentajeAvance(resultado) ;

        }
        private List<ProcesoReportabilidad> CalcularPorcentajeAvance(List<ProcesoReportabilidad> procesos)
        {
            foreach (var item in procesos)
            {
                item.PORCENTAJE_AVANCE = CalcularPorcentajeAvance(item);
            }
            return procesos;

        }
        private string CalcularPorcentajeAvance(ProcesoReportabilidad proceso)
        {
            int avance = 0;
            if (proceso.FECHA_SOCIALIZACION != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_ELEGIBILIDAD != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_REAL_PCT != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_REAL_PROT != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_REAL_PROT2 != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_REAL_EJD != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_EI != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_AUDITORIA != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_VALIDACION_CHILE_VALORA != null)
            {
                avance += 10;
            }
            if (proceso.FECHA_TERMINO != null)
            {
                avance += 10;
            }
            return avance.ToString()+"%";
        }
    }
}
