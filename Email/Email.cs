using Azure.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ExternalConnectors;
using Microsoft.Kiota.Abstractions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Email
{
    public class Email
    {
        private readonly IConfiguration _configuration;
        public Email(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private ClientSecretCredential Credential()
        {
            var graphSetting = _configuration.GetSection("GraphSetting");
            var tenantId = graphSetting.GetSection("tenantId").Value;
            var clientId = graphSetting.GetSection("clientId").Value;
            var clientSecret = graphSetting.GetSection("clientSecret").Value;
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                throw new Exception("No se encuentra Configuración para Microsoft Graph");
            }
            return new ClientSecretCredential(tenantId, clientId, clientSecret);
        }

        public async void EnviarCorreo(string EmailDestino, string asunto, string body, bool bodyAsHtml = true)
        {
            var graphClient = new GraphServiceClient(Credential());

            var requestBody = new Microsoft.Graph.Users.Item.SendMail.SendMailPostRequestBody
            {
                Message = new Message
                {
                    Subject = asunto,
                    Body = new ItemBody
                    {
                        ContentType = bodyAsHtml ? BodyType.Html : BodyType.Text,
                        Content = body,
                    },
                    ToRecipients = new List<Recipient>
        {
            new Recipient
            {
                EmailAddress = new EmailAddress
                {
                    Address = EmailDestino,
                },
            },
        },
                },
                SaveToSentItems = false,
            };
            await graphClient.Users[_configuration.GetSection("GraphSetting:emailOrigen").Value].SendMail.PostAsync(requestBody);
        }


    }
}
