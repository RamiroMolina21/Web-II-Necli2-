using Microsoft.Extensions.Configuration;
using NecliGestion.Logica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NecliGestion.Logica.Services;
        
    public class CorreoService : ICorreoService
    {
        private readonly IConfiguration _config;

        public CorreoService(IConfiguration config)
        {
            _config = config;
        }

        public void EnviarCorreo(string destinatario, string asunto, string contenidoHtml)
        {
            var smtpHost = _config["EmailSettings:SmtpHost"];
            var smtpPort = int.Parse(_config["EmailSettings:SmtpPort"]);
            var smtpUser = _config["EmailSettings:SmtpUser"];
            var smtpPass = _config["EmailSettings:SmtpPass"];
            var fromEmail = _config["EmailSettings:From"];

            using var cliente = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mensaje = new MailMessage(fromEmail, destinatario, asunto, contenidoHtml)
            {
                IsBodyHtml = true
            };

            cliente.Send(mensaje);
        }
    }
