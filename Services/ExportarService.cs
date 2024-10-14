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
using System.Data.Common;

namespace Services
{
    public class ExportarService : IExportarService
    {
        private RepositoryContext db;
        public ExportarService()
        {
            db = new RepositoryContext();
        }

        public List<Exportar> getDatosExportacion(int idPerfil, int idFaena, int idCliente)
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
                .FromSqlRaw(query, idPerfil, idFaena, idCliente)
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

        public XLWorkbook GenerarExcelBrechasCandidatos(ExcelBrechaCandidatos excelBrechaCandidatosReq)
        {
            //info Hoja 1 y 2
            DataSet tablaExcel = InfoHoja1(excelBrechaCandidatosReq.idCliente, excelBrechaCandidatosReq.idUsuario, excelBrechaCandidatosReq.idFaena, excelBrechaCandidatosReq.idPerfil, excelBrechaCandidatosReq.fechaInicio, excelBrechaCandidatosReq.fechaFin);

            if (tablaExcel.Tables[1].Rows.Count == 0)
            {
                return null;
            }

            XLWorkbook libro = GenerarHoja1y2(tablaExcel, excelBrechaCandidatosReq);

            //Get lista de los perfiles que trabaja el cliente
            DataTable dtPerfilesCliente = new DataTable();
            dtPerfilesCliente = PerfilesPorCliente(excelBrechaCandidatosReq);

            //Por cada perfil debes agregar las columnas de brechas, N * Candidatos
            DataTable dtBrechasPorPerfilXCandidato = new DataTable();
            DataTable dtBrechasCandidatosPorPerfil = new DataTable();


            for (int i = 0; i < dtPerfilesCliente.Rows.Count; i++)
            {
                dtBrechasPorPerfilXCandidato.Columns.Clear();
                dtBrechasPorPerfilXCandidato.Clear();
                dtBrechasPorPerfilXCandidato.Columns.Add("COLABORADOR");
                dtBrechasPorPerfilXCandidato.Columns.Add("RUN");

                // GET BRECHAS POR CADA PERFIL SEGUN CLIENTE
                DataTable dtBrechasPorPerfil = BrechasPorPerfilPorCliente(excelBrechaCandidatosReq, Convert.ToInt32(dtPerfilesCliente.Rows[i]["CRR_PERFIL"])); // retorna id y nombre del candidato

                //Añadimos cabeceras de brechas 
                for (int x = 0; x < dtBrechasPorPerfil.Rows.Count; x++)
                {
                    dtBrechasPorPerfilXCandidato.Columns.Add(dtBrechasPorPerfil.Rows[x]["GLS_BRECHA"].ToString());//TENEMOS LAS COLUMNAS BRECHAS Y CANDIDATOS
                }

                //OBTENER BRECHAS Y CANDIDATOS por un perfil
                dtBrechasCandidatosPorPerfil = BrechasDeCandidatos(excelBrechaCandidatosReq, Convert.ToInt32(dtPerfilesCliente.Rows[i]["CRR_PERFIL"]));

                //Añadimos filas como candidatos existan
                for (int x = 0; x < dtBrechasCandidatosPorPerfil.Rows.Count; x++)
                {
                    string brecha = dtBrechasCandidatosPorPerfil.Rows[x]["GLS_BRECHA"].ToString();
                    string candidato = dtBrechasCandidatosPorPerfil.Rows[x]["CANDIDATO"].ToString();
                    string runCandidato = dtBrechasCandidatosPorPerfil.Rows[x]["RUN"].ToString();

                    var k = (from r in dtBrechasPorPerfilXCandidato.Rows.OfType<DataRow>() where r["COLABORADOR"].ToString() == candidato select r).FirstOrDefault();

                    if (k == null) //SI K ES NULL SIGNIFICA QUE dtBrechasPorPerfilXCandidato AUN NO TIENE LA BRECHA
                    {
                        DataRow workRow;
                        workRow = dtBrechasPorPerfilXCandidato.NewRow();
                        workRow["COLABORADOR"] = candidato;
                        workRow["RUN"] = runCandidato;
                        workRow[dtBrechasCandidatosPorPerfil.Rows[x]["GLS_BRECHA"].ToString()] = "X";

                        dtBrechasPorPerfilXCandidato.Rows.Add(workRow);
                    }
                    else
                    {
                        k.BeginEdit();
                        k[dtBrechasCandidatosPorPerfil.Rows[x]["GLS_BRECHA"].ToString()] = "X";
                        k.EndEdit();
                    }

                }

                string nombrePerfil = dtPerfilesCliente.TableName = "BRECHAS X COLABORADOR";
                var worksheet = libro.Worksheets.Add(nombrePerfil.Replace("/", ""));

                worksheet.Cell("A1").InsertTable(tablaExcel.Tables[0]);
                worksheet.Cell("A10").InsertTable(dtBrechasPorPerfilXCandidato);

                foreach (var cell in worksheet.Row(10).Cells())
                {
                    for (int contBrecha = 0; contBrecha < dtBrechasPorPerfil.Rows.Count; contBrecha++)
                    {


                        if (dtBrechasPorPerfil.Columns.Contains("CRR_RUBRICA") && dtBrechasPorPerfil.Rows[contBrecha]["CRR_RUBRICA"].ToString() == "1")
                        {
                            if (cell.Value.ToString() == dtBrechasPorPerfil.Rows[contBrecha]["GLS_BRECHA"].ToString())
                            {
                                cell.Style.Font.SetFontColor(XLColor.White);
                            }
                        }
                        else
                        {

                            if (cell.Value.ToString() == dtBrechasPorPerfil.Rows[contBrecha]["GLS_BRECHA"].ToString() && dtBrechasPorPerfil.Rows[contBrecha]["FLG_COMP_COND_CRIT"].ToString() != "0")
                            {
                                cell.Style.Fill.SetBackgroundColor(XLColor.Red);
                                cell.Style.Font.SetFontColor(XLColor.White);
                            }

                        }




                        //if (cell.Value.ToString() == dtBrechasPorPerfil.Rows[contBrecha]["GLS_BRECHA"].ToString() &&  dtBrechasPorPerfil.Rows[contBrecha]["FLG_COMP_COND_CRIT"].ToString()!= "0")
                        //{
                        //    cell.Style.Fill.SetBackgroundColor(XLColor.Red);
                        //    cell.Style.Font.SetFontColor(XLColor.White);
                        //}
                    }
                }
                worksheet.ColumnsUsed().Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
                worksheet.Column(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                worksheet.Row(1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                worksheet.ColumnsUsed().AdjustToContents();

            }

            return libro;
        }

        #region Generación de hojas

        private XLWorkbook GenerarHoja1y2(DataSet tablaExcel, ExcelBrechaCandidatos excelBrechaCandidatosReq)
        {

            var libro = new XLWorkbook();


            DataTable dtCabeceraVigentes = tablaExcel.Tables[0];
            DataTable dtProcesosVigentes = tablaExcel.Tables[1];
            //DataTable dtCabeceraNoVigentes = tablaExcel.Tables[2];
            //DataTable dtProcesosNoVigentes = tablaExcel.Tables[3];


            dtCabeceraVigentes.TableName = "Procesos Vigentes";

            dtProcesosVigentes.TableName = "Tickets2";

            libro.Worksheets.Add(dtCabeceraVigentes);
            if (excelBrechaCandidatosReq.idFaena > 0)
            {
                dtProcesosVigentes.Columns.Remove("FAENA");
                //dtProcesosNoVigentes.Columns.Remove("FAENA");
            }
            libro.Worksheet(1).Cell(11, 1).InsertTable(dtProcesosVigentes);
            libro.Worksheet(1).ColumnsUsed().AdjustToContents();

            //hoja2
            //dtCabeceraNoVigentes.TableName = "No Vigente";

            //dtProcesosNoVigentes.TableName = "Tickets4";

            //libro.Worksheets.Add(dtCabeceraNoVigentes);
            //libro.Worksheet(2).Cell(10, 1).InsertTable(dtProcesosNoVigentes);
            //libro.Worksheet(2).ColumnsUsed().AdjustToContents();

            //Hoja4


         
        
          
       



            return libro;
        }



        #endregion


        #region llamadas base de datos 
        private DataSet InfoHoja1(int idCliente, int idUsuario, int idFaena, int idPerfil, DateTime? fechaInicio, DateTime? fechaFin)
        {
            DbCommand command = null;
            try
            {
                DbProviderFactory factory =
                    DbProviderFactories.GetFactory(db.Database.GetDbConnection());
                DbConnection connection = db.Database.GetDbConnection();
                // Define the query.
                string query = "SP_EXCEL_PORTAL_BRECHA";

                // Create the DbCommand.
                command = factory.CreateCommand();

                command.CommandText = query;
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                var idClienteParameter = command.CreateParameter();
                idClienteParameter.ParameterName = "CLIENTE";
                idClienteParameter.Value = idCliente;
                command.Parameters.Add(idClienteParameter);
                var idUsuarioParameter = command.CreateParameter();
                idUsuarioParameter.ParameterName = "USUARIO";
                idUsuarioParameter.Value = idUsuario;
                command.Parameters.Add(idUsuarioParameter);
                var idFaenaParameter = command.CreateParameter();
                idFaenaParameter.ParameterName = "FAENA";
                idFaenaParameter.Value = idFaena;
                command.Parameters.Add(idFaenaParameter);
                var idPerfilParameter = command.CreateParameter();
                idPerfilParameter.ParameterName = "PERFIL";
                idPerfilParameter.Value = idPerfil;
                command.Parameters.Add(idPerfilParameter);


                var fechaInicioParameter = command.CreateParameter();
                fechaInicioParameter.ParameterName = "FECHA_INICIO";
                fechaInicioParameter.Value = fechaInicio;
                command.Parameters.Add(fechaInicioParameter);



                var fechaFinParameter = command.CreateParameter();
                fechaFinParameter.ParameterName = "FECHA_FIN";
                fechaFinParameter.Value = fechaFin;
                command.Parameters.Add(fechaFinParameter);



                command.Connection.Open();
                // Create the DbDataAdapter.
                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = command;

                // Fill the DataTable.
                DataSet table = new DataSet();
                adapter.Fill(table);

                return table;

            }
            catch (Exception ex)
            {
                throw ex;
                return null;

            }
            finally
            {
                command.Connection.Close();

            }

        }

        private DataTable PerfilesPorCliente(ExcelBrechaCandidatos excelBrechaCandidatos)
        {
            DbCommand command = null;

            try
            {
                DbProviderFactory factory =
                    DbProviderFactories.GetFactory(db.Database.GetDbConnection());

                DbConnection connection = db.Database.GetDbConnection();

                // Define the query.
                string query = @"select distinct
                                     te.CRR_PERFIL,
                                     tp.DESC_PERFIL
                                     from tges_evaluacion te
                                     join tg_perfil tp on(te.CRR_PERFIL = tp.CRR_IDPERFIL)
                                     where te.FLG_PROCESO_ANULADO = 0 AND te.FLG_PROCESO_ANTIGUO = 0 and 
                                     CRR_CLIENTE =  @CLIENTE and CRR_IDPERFIL =  @PERFIL ";

                query += excelBrechaCandidatos.idFaena > 0 ? " and CRR_FAENA = @FAENA" : "";

                if (excelBrechaCandidatos.fechaInicio.HasValue && excelBrechaCandidatos.fechaFin.HasValue)
                {
                    query += " and te.FECHA_INFORME  >= @FECHAINICIO and te.FECHA_INFORME  <= @FECHAFIN";
                }


                // Create the DbCommand.
                command = factory.CreateCommand();
                command.CommandText = query;
                command.Connection = connection;
                var idClienteParameter = command.CreateParameter();
                idClienteParameter.ParameterName = "CLIENTE";
                idClienteParameter.Value = excelBrechaCandidatos.idCliente;
                command.Parameters.Add(idClienteParameter);

                if (excelBrechaCandidatos.idPerfil > 0)
                {
                    var idPerfilParameter = command.CreateParameter();
                    idPerfilParameter.ParameterName = "PERFIL";
                    idPerfilParameter.Value = excelBrechaCandidatos.idPerfil;
                    command.Parameters.Add(idPerfilParameter);
                }
                if (excelBrechaCandidatos.idFaena > 0)
                {
                    var idFaenaParameter = command.CreateParameter();
                    idFaenaParameter.ParameterName = "FAENA";
                    idFaenaParameter.Value = excelBrechaCandidatos.idFaena;
                    command.Parameters.Add(idFaenaParameter);
                }

                if (excelBrechaCandidatos.fechaInicio.HasValue && excelBrechaCandidatos.fechaFin.HasValue)
                {
                    var fechaInicioParameter = command.CreateParameter();
                    fechaInicioParameter.ParameterName = "FECHAINICIO"; 
                    fechaInicioParameter.Value = excelBrechaCandidatos.fechaInicio.Value;
                    command.Parameters.Add(fechaInicioParameter);

                    var fechaFinParameter = command.CreateParameter();
                    fechaFinParameter.ParameterName = "FECHAFIN";
                    fechaFinParameter.Value = excelBrechaCandidatos.fechaFin.Value;
                    command.Parameters.Add(fechaFinParameter);
                }

                command.Connection.Open();
                // Create the DbDataAdapter.
                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = command;

                // Fill the DataTable.
                DataSet tables = new DataSet();
                adapter.Fill(tables);
                return tables.Tables[0];


            }
            catch (Exception ex)
            {
                throw ex;
                return null;

            }
            finally
            {
                command.Connection.Close();

            }
        }

        private DataTable BrechasDeCandidatos(ExcelBrechaCandidatos excelBrechaCandidatos, int idPerfil = 0)
        {
            DbCommand command = null;

            try
            {
                DbProviderFactory factory =
                    DbProviderFactories.GetFactory(db.Database.GetDbConnection());
                DbConnection connection = db.Database.GetDbConnection();
                // Define the query.

                string query = @"select
                                 tdi.CRR_IDDETINSTRUMENTO,
                                 tdi.GLS_BRECHA,
                                 te.CRR_CANDIDATO,
                                 concat(tc.NOMBRE_CANDIDATO, ' ', tc.APELLIDOS_CANDIDATO) CANDIDATO,
                                 concat(tc.RUN_CANDIDATO, '-', tc.DIG_CANDIDATO) RUN

                                from tges_evaluacion te
                                 join tg_candidato tc on (te.CRR_CANDIDATO = tc.CRR_IDCANDIDATO)
                                 join tges_eval_pct tep on (te.CRR_IDEVALUACION = tep.CRR_EVALUACION)
                                 join tcnf_det_instrumento tdi on (tdi.CRR_IDDETINSTRUMENTO = tep.CRR_DETINSTRUMENTO)

                                where te.FLG_PROCESO_ANULADO = 0 AND te.FLG_PROCESO_ANTIGUO = 0 and tep.FLG_BRECHA <> 0 and 
                                 te.CRR_PERFIL = @PERFIL and te.CRR_CLIENTE = @CLIENTE ";

                query += excelBrechaCandidatos.idFaena > 0 ? @" and  te.CRR_FAENA = @FAENA" : "";

                if (excelBrechaCandidatos.fechaInicio.HasValue && excelBrechaCandidatos.fechaFin.HasValue)
                {
                    query += @" and (
                    te.FECHA_INFORME >= @FECHAINICIO AND te.FECHA_INFORME <= @FECHAFIN
                    OR (te.FECHA_INFORME >= @FECHAINICIO AND @FECHAFIN IS NULL)
                     )";
                }



                query += @" group by tdi.GLS_BRECHA, CRR_CANDIDATO order by CANDIDATO ASC;";



                // Create the DbCommand.
                command = factory.CreateCommand();
                command.CommandText = query;
                command.Connection = connection;
                var idClienteParameter = command.CreateParameter();
                idClienteParameter.ParameterName = "CLIENTE";
                idClienteParameter.Value = excelBrechaCandidatos.idCliente;
                command.Parameters.Add(idClienteParameter);

                if (excelBrechaCandidatos.idPerfil > 0 || idPerfil > 0)
                {
                    var idPerfilParameter = command.CreateParameter();
                    idPerfilParameter.ParameterName = "PERFIL";
                    idPerfilParameter.Value = excelBrechaCandidatos.idPerfil > 0 ? excelBrechaCandidatos.idPerfil : idPerfil;
                    command.Parameters.Add(idPerfilParameter);
                }

                if (excelBrechaCandidatos.idFaena > 0)
                {
                    var idFaenaParameter = command.CreateParameter();
                    idFaenaParameter.ParameterName = "FAENA";
                    idFaenaParameter.Value = excelBrechaCandidatos.idFaena;
                    command.Parameters.Add(idFaenaParameter);
                }

                if (excelBrechaCandidatos.fechaInicio.HasValue && excelBrechaCandidatos.fechaFin.HasValue)
                {
                    var fechaInicioParameter = command.CreateParameter();
                    fechaInicioParameter.ParameterName = "FECHAINICIO";
                    fechaInicioParameter.Value = excelBrechaCandidatos.fechaInicio.Value;
                    command.Parameters.Add(fechaInicioParameter);

                    var fechaFinParameter = command.CreateParameter();
                    fechaFinParameter.ParameterName = "FECHAFIN";
                    fechaFinParameter.Value = excelBrechaCandidatos.fechaFin.Value;
                    command.Parameters.Add(fechaFinParameter);
                }

                command.Connection.Open();
                // Create the DbDataAdapter.
                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = command;

                // Fill the DataTable.
                DataSet tables = new DataSet();
                adapter.Fill(tables);
                command.Connection.Close();
                return tables.Tables[0];

            }
            catch (Exception ex)
            {
                throw ex;
                return null;

            }
            finally
            {
                command.Connection.Close();

            }
        }

        private DataTable CandidatosPorPerfilPorCliente(ExcelBrechaCandidatos excelBrechaCandidatos, int idPerfil = 0)
        {
            DbCommand command = null;

            try
            {
                DbProviderFactory factory =
                    DbProviderFactories.GetFactory(db.Database.GetDbConnection());
                DbConnection connection = db.Database.GetDbConnection();
                // Define the query.
                string query = @"select distinct
                                     te.CRR_CANDIDATO,
                                     concat(tc.NOMBRE_CANDIDATO, ' ', tc.APELLIDOS_CANDIDATO) CANDIDATO
                                     from tges_evaluacion te  
                                     join tg_candidato tc on (te.CRR_CANDIDATO = tc.CRR_IDCANDIDATO)
                                     where te.FLG_PROCESO_ANULADO = 0 AND te.FLG_PROCESO_ANTIGUO = 0 
                                     and te.CRR_CLIENTE = @CLIENTE and te.CRR_PERFIL = @PERFIL and te.CRR_FAENA = @FAENA ;";


                if (excelBrechaCandidatos.fechaInicio.HasValue)
                {
                    query += " and te.FECHA_INFORME >= @FECHAINICIO";
                }

                if (excelBrechaCandidatos.fechaFin.HasValue)
                {
                    query += " and (te.FECHA_INFORME <= @FECHAFIN OR @FECHAFIN IS NULL)";
                }

                // Create the DbCommand.
                command = factory.CreateCommand();
                command.CommandText = query;
                command.Connection = connection;
                var idClienteParameter = command.CreateParameter();
                idClienteParameter.ParameterName = "CLIENTE";
                idClienteParameter.Value = excelBrechaCandidatos.idCliente;
                command.Parameters.Add(idClienteParameter);

                if (excelBrechaCandidatos.idPerfil > 0 || idPerfil > 0)
                {
                    var idPerfilParameter = command.CreateParameter();
                    idPerfilParameter.ParameterName = "PERFIL";
                    idPerfilParameter.Value = excelBrechaCandidatos.idPerfil > 0 ? excelBrechaCandidatos.idPerfil : idPerfil;
                    command.Parameters.Add(idPerfilParameter);
                }

                if (excelBrechaCandidatos.idFaena > 0)
                {
                    var idFaenaParameter = command.CreateParameter();
                    idFaenaParameter.ParameterName = "FAENA";
                    idFaenaParameter.Value = excelBrechaCandidatos.idFaena;
                    command.Parameters.Add(idFaenaParameter);
                }


                if (excelBrechaCandidatos.fechaInicio.HasValue)
                {
                    var fechaInicioParameter = command.CreateParameter();
                    fechaInicioParameter.ParameterName = "FECHAINICIO";
                    fechaInicioParameter.Value = excelBrechaCandidatos.fechaInicio.Value;
                    command.Parameters.Add(fechaInicioParameter);
                }

                if (excelBrechaCandidatos.fechaFin.HasValue)
                {
                    var fechaFinParameter = command.CreateParameter();
                    fechaFinParameter.ParameterName = "FECHAFIN";
                    fechaFinParameter.Value = excelBrechaCandidatos.fechaFin.Value;
                    command.Parameters.Add(fechaFinParameter);
                }


                command.Connection.Open();
                // Create the DbDataAdapter.
                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = command;

                // Fill the DataTable.
                DataSet tables = new DataSet();
                adapter.Fill(tables);
                return tables.Tables[0];
                command.Connection.Close();


            }
            catch (Exception ex)
            {
                throw ex;
                return null;

            }
            finally
            {
                command.Connection.Close();

            }
        }


        private DataTable BrechasPorPerfilPorCliente(ExcelBrechaCandidatos excelBrechaCandidatos, int idPerfil = 0)
        {
            DbCommand command = null;

            try
            {
                DbProviderFactory factory =
                    DbProviderFactories.GetFactory(db.Database.GetDbConnection());
                DbConnection connection = db.Database.GetDbConnection();
                // Define the query.
                string query = @"select distinct
                                 tdi.GLS_BRECHA,
                                 tdi.CRR_IDDETINSTRUMENTO,
                                 tdi.FLG_COMP_COND_CRIT,
                                 te.CRR_RUBRICA
                                 from tges_evaluacion te
                                 join tges_eval_pct tep on (te.CRR_IDEVALUACION = tep.CRR_EVALUACION)
                                 join tcnf_det_instrumento tdi on (tdi.CRR_IDDETINSTRUMENTO = tep.CRR_DETINSTRUMENTO)
                
                                where te.FLG_PROCESO_ANULADO = 0 and  tep.FLG_BRECHA <> 0 and 
                                 te.CRR_CLIENTE = @CLIENTE and te.CRR_PERFIL = @PERFIL  ";


                query += excelBrechaCandidatos.idFaena > 0 ? " and te.CRR_FAENA = @FAENA" : "";

                if (excelBrechaCandidatos.fechaInicio.HasValue && excelBrechaCandidatos.fechaFin.HasValue)
                {
                    query += @" and (
                       (te.FECHA_INFORME >= @FECHAINICIO AND te.FECHA_INFORME <= @FECHAFIN)
                          OR (te.FECHA_INFORME >= @FECHAINICIO AND @FECHAFIN IS NULL)
                        )";
                }
                query += " group by tdi.GLS_BRECHA";

                // Create the DbCommand.
                command = factory.CreateCommand();
                command.CommandText = query;
                command.Connection = connection;
                var idClienteParameter = command.CreateParameter();
                idClienteParameter.ParameterName = "CLIENTE";
                idClienteParameter.Value = excelBrechaCandidatos.idCliente;
                command.Parameters.Add(idClienteParameter);

                if (excelBrechaCandidatos.idPerfil > 0 || idPerfil > 0)
                {
                    var idPerfilParameter = command.CreateParameter();
                    idPerfilParameter.ParameterName = "PERFIL";
                    idPerfilParameter.Value = excelBrechaCandidatos.idPerfil > 0 ? excelBrechaCandidatos.idPerfil : idPerfil;
                    command.Parameters.Add(idPerfilParameter);
                }

                if (excelBrechaCandidatos.idFaena > 0)
                {
                    var idFaenaParameter = command.CreateParameter();
                    idFaenaParameter.ParameterName = "FAENA";
                    idFaenaParameter.Value = excelBrechaCandidatos.idFaena;
                    command.Parameters.Add(idFaenaParameter);
                }

                if (excelBrechaCandidatos.fechaInicio.HasValue && excelBrechaCandidatos.fechaFin.HasValue)
                {
                    var fechaInicioParameter = command.CreateParameter();
                    fechaInicioParameter.ParameterName = "FECHAINICIO";
                    fechaInicioParameter.Value = excelBrechaCandidatos.fechaInicio.Value;
                    command.Parameters.Add(fechaInicioParameter);

                    var fechaFinParameter = command.CreateParameter();
                    fechaFinParameter.ParameterName = "FECHAFIN";
                    fechaFinParameter.Value = excelBrechaCandidatos.fechaFin.Value;
                    command.Parameters.Add(fechaFinParameter);
                }

                command.Connection.Open();
                // Create the DbDataAdapter.
                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = command;

                // Fill the DataTable.
                DataSet tables = new DataSet();
                adapter.Fill(tables);
                return tables.Tables[0];


            }
            catch (Exception ex)
            {
                throw ex;
                return null;

            }
            finally
            {
                command.Connection.Close();

            }
        }

        #endregion


    }

}

