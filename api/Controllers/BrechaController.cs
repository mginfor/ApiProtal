using api.Helpers;
using Contracts;
using Entities.DbModels;
using Entities.EPModels;
using iText.Html2pdf;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Utils;
using iText.Layout;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace api.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class BrechaController : Controller
    {
        private IEvaluacionPCTService _evaluacionPctService;
        private IEvaluacionPROTService _evaluacionProtService;
        private IEvaluacionEIService _evaluacionEiService;
        private IProcesoService _procesoService;
        private IDocumentoBrechaService _documentoBrechaService;
        public BrechaController(IEvaluacionPCTService evaluacionPCTService,
            IEvaluacionPROTService evaluacionPROTService,
            IEvaluacionEIService evaluacionEIService,
            IProcesoService procesoService,
            IDocumentoBrechaService documentoBrechaService)
        {
            _evaluacionPctService = evaluacionPCTService;
            _evaluacionProtService = evaluacionPROTService;
            _evaluacionEiService = evaluacionEIService;
            _procesoService = procesoService;
            _documentoBrechaService = documentoBrechaService;

        }

        [Route("[action]/{idEvaluacion}")]
        [HttpGet]
        public IActionResult getDataBrechasByIdEvaluacion(int idEvaluacion)
        {
            var salida = new GenericResponse();
            var brechaPCT = _evaluacionPctService.getBrechasByIdEvaluacion(idEvaluacion);
            var brechaPROT = _evaluacionProtService.getBrechasByIdEvaluacion(idEvaluacion);
            var brechaEI = _evaluacionEiService.getBrechasByIdEvaluacion(idEvaluacion);

            if (idEvaluacion > 0)
            {
                salida.data = new
                {
                    brechaPct = brechaPCT,
                    brechaProt = brechaPROT,
                    brechaEi = brechaEI
                };
                return Ok(salida);
            }
            else
            {
                salida.status = false;
                salida.data = new { message = "No Folio" };
                return BadRequest(salida);
            }
        }

        [Route("[action]/{idEvaluacion}")]
        [HttpGet]
        public IActionResult getDataProcesoByIdEvaluacion(int idEvaluacion)
        {
            var salida = new GenericResponse();
            var resultado = _procesoService.getProcesosByIdEvaluacion(idEvaluacion).FirstOrDefault();
            if (idEvaluacion > 0)
            {
                if (resultado == null)
                {
                    salida.status = false;
                    salida.data = new { message = "Evaluacion no Encontrada" };
                    return BadRequest(salida);
                }
                else
                {
                    salida.data = resultado;
                    return Ok(salida);
                }

            }
            else
            {
                salida.status = false;
                salida.data = new { message = "No Evaluacion" };
                return BadRequest(salida);
            }
        }
        /// <summary>
        /// recibe enum, nombre archivo y string de archivo base64
        /// [Enum]= PdfAcreditaciones = 0, PdfCotizaciones = 1, Img = 2, Brechas = 3
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpPost("postSubeArchivo")]
        public async Task<IActionResult> postSubeArchivo([FromBody] SharePointFileMap archivo)
        {
            var salida = new GenericResponse();
            string relativePath = fromBase64(archivo.attName, archivo.att);
            var resultado = await FileServerHelper.UploadFileToSharePoint((FileServerHelper.Libreria)archivo.libreria, relativePath);
            salida.data = new { archivoName = archivo.attName, archivoRuta = resultado + "/" + archivo.attName };
            System.IO.File.Delete(relativePath);
            return Ok(salida);
        }

        /// <summary>
        /// recibe enum, nombre archivo y ruta sharepoint de archivo
        /// [Enum]= PdfAcreditaciones = 0, PdfCotizaciones = 1, Img = 2, Brechas = 3
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpPost("postBajaArchivo")]
        public async Task<IActionResult> postBajaArchivo([FromBody] SharePointFileMap archivo)
        {
            var salida = new GenericResponse();
            var resultado = await FileServerHelper.DownloadFileFromSharePoint((FileServerHelper.Libreria)3, archivo.attRuta);
            salida.data = new { archivoName = archivo.attName, archivoB64 = toBase64(resultado) };
            return Ok(salida);
        }

        /// <summary>
        /// recibe idEvaluacion para Informe Levantamiento
        /// [Enum]= PdfAcreditaciones = 0, PdfCotizaciones = 1, Img = 2, Brechas = 3
        /// </summary>
        /// <param name="archivo"></param>
        /// <returns></returns>
        [HttpPost("postGenerateInformeLevantamiento")]
        public async Task<IActionResult> postGenerateInformeLevantamiento([FromBody] InformeLevantamiento levantamiento)
        {
            var salida = new GenericResponse();
            var documentos = _documentoBrechaService.getAllDocumentosByIdEvaluacion(levantamiento.idEvaluacion);
            List<string> adjuntos = new List<string>();
            foreach (var archivo in documentos)
            {
                adjuntos.Add(await FileServerHelper.DownloadFileFromSharePoint((FileServerHelper.Libreria)3, archivo.ruta));
            }

            var pdfInforme = generatePdf(levantamiento.htmlDocument, levantamiento.idEvaluacion);
            var rutaHtml = @"Plantillas\Informe_" + levantamiento.idEvaluacion + ".html";
            var pdfFinal=mergePdf(levantamiento.idEvaluacion, pdfInforme,adjuntos);

            salida.data = new { archivoName = "InformeLevantamiento_"+ levantamiento.idEvaluacion, archivoB64 = toBase64(pdfFinal) };

            System.IO.File.Delete(pdfInforme);
            System.IO.File.Delete(rutaHtml);
            foreach (var item in adjuntos)
            {
                System.IO.File.Delete(item);
            }

            return Ok(salida);
        }

        [HttpPost("postAsignaLevantamiento")]
        public IActionResult postAsignaLevantamiento([FromBody] LevantamientoMap levantamiento)
        {
            var salida = new GenericResponse();
            var documento = new DocumentoBrecha
            {
                idEvaluacion = levantamiento.idEvaluacion,
                nombreDocumento = levantamiento.nombreArchivo,
                medioTratamiento = levantamiento.tratamiento,
                entidadTratamiento = levantamiento.entidad,
                ruta = levantamiento.rutaDocumento
            };

            var resultado = _documentoBrechaService.saveDocumento(documento);
            foreach (var item in levantamiento.brechas)
            {
                switch (item.tipoBrecha)
                {
                    case "PCT":
                        _evaluacionPctService.addDocumentoInBrecha(item.idBrecha, resultado.id);
                        break;
                    case "PROT":
                        _evaluacionProtService.addDocumentoInBrecha(item.idBrecha, resultado.id);
                        break;
                    case "EI":
                        _evaluacionEiService.addDocumentoInBrecha(item.idBrecha, resultado.id);
                        break;
                    default:
                        break;
                }
            }
            salida.data = resultado;
            return Ok(salida);
        }

        private string generatePdf(string html,int idEvaluacion)
        {
            var rutaPdf = @"Plantillas\Informe_" + idEvaluacion +".pdf";
            var rutaHtml = @"Plantillas\Informe_" + idEvaluacion + ".html";

            System.IO.File.WriteAllText(rutaHtml, html);

            ConverterProperties converterProperties = new ConverterProperties();
            converterProperties.SetCharset("UTF-8");
            PdfWriter writer = new PdfWriter(rutaPdf);
            PdfDocument pdf = new PdfDocument(writer);
            pdf.SetDefaultPageSize(PageSize.LETTER);
            pdf.SetTagged();

            var fstream = new FileStream(rutaHtml, FileMode.OpenOrCreate);

            HtmlConverter.ConvertToPdf(fstream, pdf, converterProperties);
            pdf.Close();
            writer.Close();
            fstream.Close();
            return rutaPdf;
        }

        private string mergePdf(int idEvaluacion, string rutaPdfInforme, List<string> adjuntos)
        {
            var ruta = @"Plantillas\InformeLevantamiento_" + idEvaluacion + ".pdf";

            PdfDocument pdf = new PdfDocument(new PdfWriter(ruta));
            PdfMerger merger = new PdfMerger(pdf);

            PdfDocument firstSourcePdf = new PdfDocument(new PdfReader(rutaPdfInforme));
            merger.Merge(firstSourcePdf, 1, firstSourcePdf.GetNumberOfPages());

            foreach (var item in adjuntos)
            {
                PdfDocument secondSourcePdf = new PdfDocument(new PdfReader(item));
                merger.Merge(secondSourcePdf, 1, secondSourcePdf.GetNumberOfPages());
                secondSourcePdf.Close();
            }

            firstSourcePdf.Close();
            
            pdf.Close();
            return ruta;
        }

        private string toBase64(string ruta)
        {
            Byte[] bytes = System.IO.File.ReadAllBytes(ruta);
            String file = Convert.ToBase64String(bytes);
            System.IO.File.Delete(ruta);
            return file;
        }
        private string fromBase64(string nombre, string att)
        {
            var ruta = @"Plantillas\" + nombre;
            var strParse = "";

            if (att.IndexOf("base64,") > 0) strParse = att.Substring(att.IndexOf("base64,") + 7);
            else strParse = att;

            var att2 = new MemoryStream(Convert.FromBase64String(strParse));
            FileStream file = new FileStream(ruta, FileMode.Create, FileAccess.Write);
            att2.WriteTo(file);
            file.Close();
            att2.Close();

            return ruta;
        }
    }
}
