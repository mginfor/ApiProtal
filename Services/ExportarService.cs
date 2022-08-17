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
using System.IO;
using ClosedXML.Excel;
using System.Data;
using MySqlConnector;
using System.Data.SqlClient;

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

        public XLWorkbook GenerarExcelBrechasCandidatos(ExcelBrechaCandidatos excelBrechaCandidatos)
        {
            //info Hoja 1

            using (var libro = new XLWorkbook())

            {

                DataSet tablaExcel = InfoHoja1(excelBrechaCandidatos.idCliente, excelBrechaCandidatos.idUsuario, excelBrechaCandidatos.idFaena, excelBrechaCandidatos.idPerfil);
                //hoja 1

                DataTable tablita1 = new DataTable();
                tablita1 = tablaExcel.Tables[0];
                tablita1.TableName = "Tickets";



                DataTable tablita2 = new DataTable();
                tablita2 = tablaExcel.Tables[1];
                tablita2.TableName = "Tickets2";
                //hoja = libro.Worksheets.Add(tablita2);
                //hoja.ColumnsUsed().AdjustToContents();

                DataTable tablita3 = new DataTable();
                tablita3 = tablaExcel.Tables[2];
                tablita3.TableName = "Tickets3";



                DataTable tablita4 = new DataTable();
                tablita4 = tablaExcel.Tables[3];
                tablita4.TableName = "Tickets4";



                //hoja 1
                libro.Worksheets.Add(tablita1);
                libro.Worksheet(1).Cell(10, 1).InsertTable(tablita2);
                libro.Worksheet(1).ColumnsUsed().AdjustToContents();

                //hoja2
                libro.Worksheets.Add(tablita3);
                libro.Worksheet(2).Cell(10, 1).InsertTable(tablita4);
                libro.Worksheet(2).ColumnsUsed().AdjustToContents();




                ////Get lista de los perfiles que trabaja el cliente
                //DataTable dtPerfilesCliente = new DataTable();
                //dtPerfilesCliente = bu.PerfilesPorCliente(189);



                ////Por cada perfil debes agregar las columnas de brechas, N * Candidatos
                //DataTable dtBrechasPorPerfilXCandidato = new DataTable();
                //DataTable dtBRECHASDECANDIDATOS = new DataTable();
                //var workbook = new XLWorkbook();//LIBREO EXCEL



                //for (int i = 0; i < dtPerfilesCliente.Rows.Count; i++)
                //{
                //    dtBrechasPorPerfilXCandidato.Columns.Clear();
                //    dtBrechasPorPerfilXCandidato.Clear();



                //    dtBrechasPorPerfilXCandidato.Columns.Add("BRECHAS");



                //    //GET CANDIDATOS POR CADA PERFIL SEGUN CLIENTE
                //    DataTable dtCandidatosPorPerfil = new DataTable();
                //    dtCandidatosPorPerfil = bu.CandidatosPorPerfilPorCliente(189, Convert.ToInt32(dtPerfilesCliente.Rows[i]["CRR_PERFIL"]));



                //    for (int j = 0; j < dtCandidatosPorPerfil.Rows.Count; j++)
                //    {
                //        dtBrechasPorPerfilXCandidato.Columns.Add(dtCandidatosPorPerfil.Rows[j]["CANDIDATO"].ToString());//TENEMOS LAS COLUMNAS BRECHAS Y CANDIDATOS
                //    }



                //    //OBTENER BRECHAS Y CANDIDATOS
                //    dtBRECHASDECANDIDATOS = bu.BrechasDeCandidatos(189, Convert.ToInt32(dtPerfilesCliente.Rows[i]["CRR_PERFIL"]));
                //    for (int x = 0; x < dtBRECHASDECANDIDATOS.Rows.Count; x++)
                //    {
                //        var k = (from r in dtBrechasPorPerfilXCandidato.Rows.OfType<DataRow>() where r["BRECHAS"].ToString() == dtBRECHASDECANDIDATOS.Rows[x]["GLS_BRECHA"].ToString() select r).FirstOrDefault();

                //        if (k == null)//SI K ES NULL SIGNIFICA QUE dtBrechasPorPerfilXCandidato AUN NO TIENE LA BRECHA
                //        {
                //            DataRow workRow;
                //            workRow = dtBrechasPorPerfilXCandidato.NewRow();
                //            workRow["BRECHAS"] = dtBRECHASDECANDIDATOS.Rows[x]["GLS_BRECHA"];
                //            workRow[dtBRECHASDECANDIDATOS.Rows[x]["CANDIDATO"].ToString()] = "X";
                //            dtBrechasPorPerfilXCandidato.Rows.Add(workRow);
                //        }
                //        else
                //        {
                //            k.BeginEdit();
                //            k[dtBRECHASDECANDIDATOS.Rows[x]["CANDIDATO"].ToString()] = "X";
                //            k.EndEdit();
                //        }
                //    }



                //    var worksheet = workbook.Worksheets.Add(dtPerfilesCliente.Rows[i]["CRR_PERFIL"].ToString());
                //    // WORKSHEET AGREGAR TABLA CON NOMBRE PERFIL Y DESPUES LA TABLA DINAMICA
                //    worksheet.Cell("A1").Value = "Perfil";
                //    worksheet.Cell("A2").Value = dtPerfilesCliente.Rows[i]["DESC_PERFIL"].ToString();
                //    worksheet.Cell("A3").InsertTable(dtBrechasPorPerfilXCandidato);
                //    worksheet.ColumnsUsed().AdjustToContents();
                //}

                return libro;

            }


        }


        public DataSet InfoHoja1(int idCliente, int idUsuario, int idFaena, int idPerfil)
        {
            DataSet tablaExcel = new DataSet();
            string query = "SP_EXCEL_PORTAL_BRECHA";
            using (var ctx = new RepositoryContext())
            {
                var cmd = ctx.Database.GetDbConnection().CreateCommand();
                cmd.CommandText = query;
                cmd.CommandType = CommandType.StoredProcedure;
                var idClienteParameter = cmd.CreateParameter();
                idClienteParameter.ParameterName = "idCliente";
                idClienteParameter.Value = idCliente;
                cmd.Parameters.Add(idClienteParameter);
                var idUsuarioParameter = cmd.CreateParameter();
                idUsuarioParameter.ParameterName = "idUsuario";
                idUsuarioParameter.Value = idUsuario;
                cmd.Parameters.Add(idUsuarioParameter);
                var idFaenaParameter = cmd.CreateParameter();
                idFaenaParameter.ParameterName = "idFaena";
                idFaenaParameter.Value = idFaena;
                cmd.Parameters.Add(idFaenaParameter);
                var idPerfilParameter = cmd.CreateParameter();
                idPerfilParameter.ParameterName = "idPerfil";
                idPerfilParameter.Value = idPerfil;
                cmd.Parameters.Add(idPerfilParameter);
                cmd.Connection.Open();
                MySqlDataAdapter da = new MySqlDataAdapter((MySqlCommand)cmd);
                da.Fill(tablaExcel);
            }
            return tablaExcel;

        }
        // public DataTable PerfilesPorCliente(int cliente)
        // {
        //     string query = @"select distinct
        // te.CRR_PERFIL,
        // tp.DESC_PERFIL
        // from tges_evaluacion te
        // join tg_perfil tp on(te.CRR_PERFIL = tp.CRR_IDPERFIL)
        // where CRR_CLIENTE = {0}; ";

        //     var table = new DataTable();
        //     using (var ctx = new RepositoryContext())
        //     {
        //         var cmd = ctx.Database.GetDbConnection().CreateCommand();
        //         cmd.CommandText = "Select Col1, Col2 from SomeTable";
        //         cmd.CommandType = CommandType.StoredProcedure;
        //         cmd.Connection.Open();
        //         table.Load(cmd.ExecuteReader());
        //     }

        // }
        // public DataTable BrechasDeCandidatos(int cliente, int perfil)
        // {
        //     List<MySqlParameter> pars = new List<MySqlParameter>();
        //     string query = @"select
        // tdi.CRR_IDDETINSTRUMENTO,
        // tdi.GLS_BRECHA,
        // te.CRR_CANDIDATO,
        // concat(tc.NOMBRE_CANDIDATO, ' ', tc.APELLIDOS_CANDIDATO) CANDIDATO



        //from tges_evaluacion te
        // join tg_candidato tc on (te.CRR_CANDIDATO = tc.CRR_IDCANDIDATO)
        // join tges_eval_pct tep on (te.CRR_IDEVALUACION = tep.CRR_EVALUACION)
        // join tcnf_det_instrumento tdi on (tdi.CRR_IDDETINSTRUMENTO = tep.CRR_DETINSTRUMENTO)



        //where tep.FLG_BRECHA <> 0 and te.CRR_PERFIL = @perfil and te.CRR_CLIENTE = @cliente
        // group by tdi.GLS_BRECHA, CRR_CANDIDATO;";
        //     pars.Add(new MySqlParameter("perfil", MySqlDbType.Int32) { Value = perfil });
        //     pars.Add(new MySqlParameter("cliente", MySqlDbType.Int32) { Value = cliente });



        //     return consultar(query, pars);
        // }
        // public DataTable CandidatosPorPerfilPorCliente(int cliente, int perfil)
        // {
        //     List<MySqlParameter> pars = new List<MySqlParameter>();
        //     string query = @"select distinct
        // te.CRR_CANDIDATO,
        // concat(tc.NOMBRE_CANDIDATO, ' ', tc.APELLIDOS_CANDIDATO) CANDIDATO
        // from tges_evaluacion te  
        // join tg_candidato tc on (te.CRR_CANDIDATO = tc.CRR_IDCANDIDATO)
        // where te.CRR_PERFIL = @perfil and te.CRR_CLIENTE = @cliente;";
        //     pars.Add(new MySqlParameter("perfil", MySqlDbType.Int32) { Value = perfil });
        //     pars.Add(new MySqlParameter("cliente", MySqlDbType.Int32) { Value = cliente });



        //     return consultar(query, pars);
        // }



    }

}

