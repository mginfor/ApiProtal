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
namespace Services
{
    public class ExportarService : IExportarService 
    {
        private RepositoryContext db;
        public ExportarService()
        {
            db = new RepositoryContext();
        }

        public List<Exportar> getDatosExportacion(int idPerfil,int idFaena,int idCliente)
        {
            var query = "SELECT distinct " +
            "p.DESC_PERFIL as PERFIL,tdi.LLAVE_PREGUNTA as llaveBrecha,  di.GLS_BRECHA as brecha, " +
            "CONCAT(can.NOMBRE_CANDIDATO, \" \" , can.APELLIDOS_CANDIDATO) as Trabajador " +
            "FROM tges_evaluacion ev, tges_eval_pct epct, tcnf_det_instrumento di, tg_candidato can, tg_perfil p, " +
            "tcnf_det_instrumento tdi , tg_faena tf " +
            "WHERE ev.CRR_IDEVALUACION = epct.CRR_EVALUACION " +
            "and epct.CRR_DETINSTRUMENTO = tdi.CRR_IDDETINSTRUMENTO " +
            "AND di.CRR_IDDETINSTRUMENTO = epct.CRR_DETINSTRUMENTO " +
            "AND ev.FLG_PROCESO_ANULADO = 0 " +
            "AND ev.FLG_CIERRE_ADM = 0 " +
            "AND ev.COD_ESTADO_PROCESO <> 0 " +
            "AND epct.FLG_ANULADO = 0 " +
            "AND epct.FLG_BRECHA <> 0 " +
            "AND ev.FECHA_VIGENCIAINFORME >= NOW() " +
            "AND can.CRR_IDCANDIDATO = ev.CRR_CANDIDATO " +
            "and ev.CRR_PERFIL = p.CRR_IDPERFIL " +
            "and ev.CRR_FAENA = tf.CRR_IDFAENA " +
            "and ev.CRR_PERFIL = {0} " +
            "and ev.CRR_FAENA  = {1} " +
            "and ev.CRR_CLIENTE = {2};";

            var salida = db.Exportar
                .FromSqlRaw(query, idPerfil, idFaena,idCliente)
                .ToList();

            return salida;
        }

        public List<BrechasCandidato> getProcesoBrecha(int idEvaluacion)
        {
            var query = "SELECT distinct " +
                        "pp.GLS_BRECHA AS BRECHA, " +
                        "if(epct.CRR_DOCUMENTOBRECHA>0, \"SI\", \"NO\") as Brecha_Tratada " +
                        "FROM tges_evaluacion ev, tges_eval_pct epct, tcnf_pool_preguntas pp, tcnf_det_instrumento di " +
                        "WHERE ev.CRR_IDEVALUACION = epct.CRR_EVALUACION " +
                        "AND ev.FLG_PROCESO_ANULADO = 0 " +
                        "AND ev.FLG_CIERRE_ADM = 0 " +
                        "AND ev.COD_ESTADO_PROCESO <> 0 " +
                        "AND epct.FLG_ANULADO = 0 " +
                        "AND epct.FLG_BRECHA <> 0 " +
                        "AND ev.FECHA_VIGENCIAINFORME >= NOW() " +
                        "and ev.CRR_IDEVALUACION = {0} " +
                        "AND di.CRR_IDDETINSTRUMENTO = epct.CRR_DETINSTRUMENTO " +
                        "AND pp.CRR_IDPOOLPREGUNTA = di.CRR_IDPOOLPREGUNTA " +
                        "ORDER BY 1; ";

            return db.brechasCandidato
                .FromSqlRaw(query, idEvaluacion)
                .ToList();
        }











    }
}
