using Email;
using Entities.EPModels;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace api.Helpers
{
    public class MailHelper
    {
        private readonly IConfiguration _configuration;
        public MailHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool sendMail(MailMap mensaje)
        {
            var mailMesssaje = generateMail(mensaje.Subject, mensaje.Messaje, mensaje.To, mensaje.Cc, mensaje.Cco, mensaje.Attachment);

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    var mailSettings = _configuration.GetSection("MailSettings");
                    client.Connect(mailSettings.GetSection("SmtpServer").Value, int.Parse(mailSettings.GetSection("SmtpPort").Value), SecureSocketOptions.StartTls);
                    client.Authenticate(mailSettings.GetSection("mailSenderAddress").Value, mailSettings.GetSection("mailSenderPass").Value);
                    client.Send(mailMesssaje);
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
                return true;
            }
        }
            
        private MimeMessage generateMail(string Subject, string messaje, List<string> To, List<string>? Cc, List<string>? Cco, AttachmentMap? Attachment)
        {
            var mailMesssaje = new MimeMessage();
            var mailSettings = _configuration.GetSection("MailSettings");

            mailMesssaje.From.Add(new MailboxAddress(mailSettings.GetSection("mailSenderName").Value, mailSettings.GetSection("mailSenderAddress").Value));

            mailMesssaje.To.AddRange(generateAddres(To));

            if (Cc?.Count > 0)
            {
                mailMesssaje.Cc.AddRange(generateAddres(Cc));
            }
            if (Cco?.Count > 0)
            {
                mailMesssaje.Bcc.AddRange(generateAddres(Cco));
            }

            mailMesssaje.Subject = Subject;
            mailMesssaje.Body = generateBody(messaje, Attachment);
            return mailMesssaje;
        }

        private List<MailboxAddress> generateAddres(List<string> Address)
        {
            List<MailboxAddress> direcciones = new List<MailboxAddress>();

            foreach (var item in Address)
            {
                MailboxAddress nuevo;
                if (MailboxAddress.TryParse(item, out nuevo))
                {
                    direcciones.Add(nuevo);
                }
            }
            return direcciones;
        }

        private MimeEntity generateBody(string body, AttachmentMap? Attachment)
        {
            var newBody = new BodyBuilder
            {
                HtmlBody = body
            };

            if (Attachment != null)
            {
                var strParse = "";

                if (Attachment.Att.IndexOf("base64,") > 0) strParse = Attachment.Att.Substring(Attachment.Att.IndexOf("base64,") + 7);
                else strParse = Attachment.Att;

                var att = new MemoryStream(Convert.FromBase64String(strParse));
                newBody.Attachments.Add(Attachment.AttName, att.ToArray());
            }

            return newBody.ToMessageBody();
        }


        public void EnviarCorreoGraph(string destinatario, string usuario, int codigo)
        {
            string relativePath = @"Plantillas\mensaje.html";
            var ruta = Path.GetFullPath(relativePath);
            string html = System.IO.File.ReadAllText(ruta);

            string body = string.Format(html, usuario, codigo.ToString());
            string asunto = "codigo de verificación: " + codigo.ToString();
            Email.Email emailGraph = new(_configuration);
            emailGraph.EnviarCorreo( destinatario, asunto, body);

        }
    }
}
