using ALISTAMIENTO_IE.DTOs;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ALISTAMIENTO_IE.Services
{
    public class EmailService
    {
        private readonly System.Net.Mail.SmtpClient _client;
        private readonly string _remitente;

        public EmailService()
        {
            _remitente = "notificaciones@integraldeempaques.com";

            _client = new System.Net.Mail.SmtpClient("192.168.16.215")
            {
                Port = 2727,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("cabana\\notificaciones", "Notifica@inte"),
                EnableSsl = false
            };
        }

        public async Task EnviarCorreoAsync(string asunto, string cuerpoHtml, string[] destinatarios)
        {
            foreach (var correo in destinatarios)
            {
                using var msg = new MailMessage(_remitente, correo)
                {
                    Subject = asunto,
                    Body = cuerpoHtml,
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };

                await _client.SendMailAsync(msg);
            }
        }
        public async Task NotificarAnulacionCamionAsync(long codCamion, string texto, string causa, string[] destinatarios)
        {
            try
            {
                string asunto = $"Camión anulado: {texto}";
                string cuerpo = $@"
            <h3>Se ha anulado un camión programado</h3>
            <p><strong>Camión:</strong> {texto}</p>
            <p><strong>Código:</strong> {codCamion}</p>
            <p><strong>Usuario:</strong> {Environment.UserName}</p>
            <p><strong>Fecha:</strong> {DateTime.Now:dd/MM/yyyy HH:mm}</p>
            <p><strong>Causa de cierre:</strong> {System.Net.WebUtility.HtmlEncode(causa)}</p>
        ";

                

                await EnviarCorreoAsync(asunto, cuerpo, destinatarios);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al enviar el correo: {ex.Message}", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
