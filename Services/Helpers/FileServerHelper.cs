using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading.Tasks;
using File = Microsoft.SharePoint.Client.File;

namespace api.Helpers
{
    public class FileServerHelper
    {
        const string Login = "no-responder@mgcertifica.cl";
        const string Password = "MaGo512!";
        const string UrlBase = @"https://mgcertifica.sharepoint.com/sites/Libreria/";
        const string UrlCotizacion = "Cotizaciones";
        const string UrlImagen = "Imagenes";
        const string UrlBrechas = "Brechas";
        string PdfAcreditacion = "";

        public FileServerHelper()
        {

        }
        public enum Libreria
        {
            PdfAcreditaciones = 0,
            PdfCotizaciones = 1,
            Img = 2,
            Brechas = 3
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DocLibrary">Enum que entrega opcion de rutas disponibles</param>
        /// <param name="FileName">ruta completa del archivo a subir</param>
        /// <param name="ClientSubFolder">Al tener este parametro crea una subCarpeta</param>
        public static async Task<string> UploadFileToSharePoint(Libreria DocLibrary, string FileName, string FechaInforme = null)
        {

            bool exito = false;
            string ruta = "";
            switch (DocLibrary)
            {
                case Libreria.PdfAcreditaciones:
                    ruta = GetRutaAC(FechaInforme);
                    break;
                case Libreria.PdfCotizaciones:
                    ruta = UrlCotizacion;
                    break;
                case Libreria.Img:
                    ruta = UrlImagen;
                    break;
                case Libreria.Brechas:
                    ruta = UrlBrechas;
                    break;
            }
            try
            {
                #region ConnectToSharePointAndInsert
                var url = new Uri(UrlBase);
                var securePassword = new SecureString();
                foreach (char c in Password)
                { securePassword.AppendChar(c); }
                // Note: The PnP Sites Core AuthenticationManager class also supports this
                using (var authenticationManager = new SharePointAuthHelper())
                using (ClientContext CContext = authenticationManager.GetContext(url, Login, securePassword))
                {
                    //CContext.Credentials = onlineCredentials;
                    Web web = CContext.Web;

                    FileCreationInformation newFile = new FileCreationInformation();
                    byte[] FileContent = System.IO.File.ReadAllBytes(FileName);
                    newFile.ContentStream = new MemoryStream(FileContent);
                    newFile.Url = Path.GetFileName(FileName);
                    newFile.Overwrite = true;

                    List DocumentLibrary = web.Lists.GetByTitle(ruta);
                    Folder Clientfolder = null;
                    Clientfolder = DocumentLibrary.RootFolder;

                    File uploadFile = Clientfolder.Files.Add(newFile);
                    CContext.Load(DocumentLibrary);
                    CContext.Load(uploadFile);
                    await CContext.ExecuteQueryAsync();
                    CContext.Dispose();
                    //Console.WriteLine("The File has been uploaded" + Environment.NewLine + "FileUrl -->" + SiteUrl + "/" + DocLibrary + "/" + ClientSubFolder + "/" + Path.GetFileName(FileName));
                    exito = true;
                }
                #endregion
            }
            catch (Exception exp)
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
                //RegistroErrores.RegistrarError(exp.ToString(), "FileServer");

            }
            finally
            {
                //Console.ReadLine();
            }
            if (exito) return UrlBase + ruta;
            else return "error";
        }

        public static async Task<string> DownloadFileFromSharePoint(Libreria DocLibrary, string FileName, string FileNameDownload = "", string FechaInforme = "")
        {
            bool exito = false;
            var saveRuta = "";
            string ruta = "";
            switch (DocLibrary)
            {
                case Libreria.PdfAcreditaciones:
                    ruta = GetRutaAC(FechaInforme);
                    break;
                case Libreria.PdfCotizaciones:
                    ruta = UrlCotizacion;
                    break;
                case Libreria.Img:
                    ruta = UrlImagen;
                    break;
                case Libreria.Brechas:
                    ruta = UrlBrechas;
                    break;
            }
            try
            {
                #region ConnectToSharePointAndInsert
                var url = new Uri(UrlBase);
                var securePassword = new SecureString();
                foreach (char c in Password)
                { securePassword.AppendChar(c); }
                // Note: The PnP Sites Core AuthenticationManager class also supports this
                using (var authenticationManager = new SharePointAuthHelper())
                using (ClientContext CContext = authenticationManager.GetContext(url, Login, securePassword))
                {
                    //CContext.Credentials = onlineCredentials;
                    //Web web = CContext.Web;
                    //CContext.Load(CContext.Web.Lists);
                    //CContext.ExecuteQuery();
                    //List DocumentLibrary = web.Lists.GetByTitle(ruta);
                    //Folder Clientfolder = null;
                    //Clientfolder = DocumentLibrary.RootFolder;
                    //Clientfolder.Update();
                    //CContext.Load(Clientfolder);
                    CContext.Load(CContext.Web, p => p.Title);
                    await CContext.ExecuteQueryAsync();
                    //FileCollection files = Clientfolder.Files;
                    //var hola = files.GetByUrl(FileName);

                    //CContext.Load(hola);
                    //await CContext.ExecuteQueryAsync();

                    ////var fileRef = listItem.GetById(FileName);
                    var file = CContext.Web.GetFileByUrl(FileName);
                    CContext.Load(file);
                    await CContext.ExecuteQueryAsync();

                    Microsoft.SharePoint.Client.ClientResult<Stream> mstream = file.OpenBinaryStream();
                    await CContext.ExecuteQueryAsync();
                    saveRuta = @"Plantillas\" + file.Name;

                    FileStream fileSave = new FileStream(saveRuta, FileMode.Create, FileAccess.Write);
                    mstream.Value.CopyTo(fileSave);
                    fileSave.Close();

                    CContext.Dispose();
                    exito = true;
                }
                #endregion
            }
            catch (Exception exp)
            {
                //Console.ForegroundColor = ConsoleColor.Red;
                //Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
                //RegistroErrores.RegistrarError(exp.ToString(), "FileServer");

            }
            finally
            {
                //Console.ReadLine();
            }

            if (exito) return saveRuta;
            else return "error";
        }



        private static string GetRutaAC(string FechaInforme)
        {
            string ruta;
            DateTime Fecha = Convert.ToDateTime(FechaInforme);
            int year = Fecha.Year;
            ruta = year.ToString();

            switch (year)
            {
                case 2019:
                    ruta += Fecha.Month < 7 ? "T1" : "T2";
                    break;
                case 2020:
                    ruta += "T1";
                    break;
                default:
                    if (Fecha.Month >= 1 && Fecha.Month <= 3)
                    {
                        ruta += "T1";
                    }
                    if (Fecha.Month > 3 && Fecha.Month <= 6)
                    {
                        ruta += "T2";
                    }
                    if (Fecha.Month > 6 && Fecha.Month <= 9)
                    {
                        ruta += "T3";
                    }
                    if (Fecha.Month > 9 && Fecha.Month <= 12)
                    {
                        ruta += "T4";
                    }
                    break;
            }

            return ruta;
        }
    }
}
