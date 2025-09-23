using ALISTAMIENTO_IE.DTOs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace ALISTAMIENTO_IE.Services
{
    public static class EmailService
    {
        // Si tienes un servicio para obtener placas por código, pásalo por parámetro (opcional).
        // De lo contrario, uso "CAMION {CodCamion}".
        public static async Task EnviarCorreosPorCamionAsync(
            IEnumerable<GrupoMovimientosDto> grupos,
            string smtpHost,
            int smtpPort,
            string smtpUser,
            string smtpPassword,
            string fromName,
            string fromEmail,
            IEnumerable<string> toRecipients,
            Func<long, Task<string?>>? getPlacasAsync = null
        )
        {
            foreach (var g in grupos)
            {
                string placas = "CAMION " + g.CodCamion;
                if (getPlacasAsync != null)
                {
                    var p = await getPlacasAsync(g.CodCamion);
                    if (!string.IsNullOrWhiteSpace(p)) placas = p!;
                }

                // Tomo conductor y documento del primer movimiento (ajusta si deseas otro criterio)
                var first = g.Movimientos.FirstOrDefault();
                string nombreConductor = first?.NOMBRE_CONDUCTOR ?? "";
                string docPrincipal = first?.NUM_DOCUMENTO ?? "";

                var msg = new MimeMessage();
                msg.From.Add(new MailboxAddress(fromName, fromEmail));
                foreach (var to in toRecipients)
                    msg.To.Add(MailboxAddress.Parse(to));

                msg.Subject = $"Programacion y Despacho camion: {placas} Conductor: {nombreConductor} de: <<{docPrincipal}>>";

                // Cuerpo HTML con tabla
                var builder = new BodyBuilder
                {
                    HtmlBody = BuildHtmlCorreo(g, placas, nombreConductor, docPrincipal)
                };

                // Adjuntar CSV
                var csvBytes = Encoding.UTF8.GetBytes(BuildCsv(g));
                builder.Attachments.Add($"Programacion despacho-{placas}-{g.Fecha:dd-MM-yyyy}.csv", csvBytes, new ContentType("text", "csv"));

                msg.Body = builder.ToMessageBody();

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(smtpHost, smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(smtpUser, smtpPassword);
                await smtp.SendAsync(msg);
                await smtp.DisconnectAsync(true);
            }
        }

        private static string BuildHtmlCorreo(GrupoMovimientosDto g, string placas, string conductor, string docPrincipal)
        {
            // Tabla con los mismos encabezados/orden que tu imagen
            var sb = new StringBuilder();
            sb.AppendLine($@"
<div style=""font-family: Segoe UI, Arial, sans-serif; font-size:13px;"">
  <p><strong>Programacion y Despacho camion:</strong> {placas} &nbsp;&nbsp;
     <strong>Conductor:</strong> {conductor} &nbsp;&nbsp;
     <strong>De:</strong> &lt;&lt;{docPrincipal}&gt;&gt;</p>
  <table cellpadding=""6"" cellspacing=""0"" style=""border-collapse:collapse; border:1px solid #999; min-width:900px;"">
    <thead>
      <tr style=""background:#f0f6fb;"">
        <th style=""border:1px solid #999;"">FECHA</th>
        <th style=""border:1px solid #999;"">NUM DOCUMENTO</th>
        <th style=""border:1px solid #999;"">ESTADO</th>
        <th style=""border:1px solid #999;"">BOD. SALIDA</th>
        <th style=""border:1px solid #999;"">BOD. ENTRADA</th>
        <th style=""border:1px solid #999;"">ITEM RESUMEN</th>
        <th style=""border:1px solid #999;"">CANT. SALDO</th>
        <th style=""border:1px solid #999;"">NOTAS DEL DOCTO</th>
      </tr>
    </thead>
    <tbody>");
            foreach (var m in g.Movimientos)
            {
                sb.AppendLine($@"
      <tr>
        <td style=""border:1px solid #999;"">{m.FECHA:dd/MM/yyyy}</td>
        <td style=""border:1px solid #999;"">{HtmlEscape(m.NUM_DOCUMENTO)}</td>
        <td style=""border:1px solid #999;"">{HtmlEscape(m.ESTADO)}</td>
        <td style=""border:1px solid #999;"">{HtmlEscape(m.BOD_SALIDA)}</td>
        <td style=""border:1px solid #999;"">{HtmlEscape(m.BOD_ENTRADA)}</td>
        <td style=""border:1px solid #999;"">{HtmlEscape(m.ITEM_RESUMEN)}</td>
        <td style=""border:1px solid #999; text-align:right;"">{m.CANT_SALDO.ToString("N3", CultureInfo.InvariantCulture)}</td>
        <td style=""border:1px solid #999;"">{HtmlEscape(m.NOTAS_DEL_DOCTO)}</td>
      </tr>");
            }
            sb.AppendLine(@"
    </tbody>
  </table>
</div>");
            return sb.ToString();
        }

        private static string BuildCsv(GrupoMovimientosDto g)
        {
            var sb = new StringBuilder();
            // Encabezados
            sb.AppendLine("FECHA,NUM DOCUMENTO,ESTADO,BOD. SALIDA,BOD. ENTRADA,ITEM RESUMEN,CANT. SALDO,NOTAS DEL DOCTO");
            foreach (var m in g.Movimientos)
            {
                sb.AppendLine(string.Join(",", new[]
                {
                m.FECHA.ToString("dd/MM/yyyy"),
                Csv(m.NUM_DOCUMENTO),
                Csv(m.ESTADO),
                Csv(m.BOD_SALIDA),
                Csv(m.BOD_ENTRADA),
                Csv(m.ITEM_RESUMEN),
                m.CANT_SALDO.ToString("0.###", CultureInfo.InvariantCulture),
                Csv(m.NOTAS_DEL_DOCTO)
            }));
            }
            return sb.ToString();
        }

        private static string Csv(string? v)
            => "\"" + (v ?? "").Replace("\"", "\"\"") + "\"";

        private static string HtmlEscape(string? v)
            => System.Net.WebUtility.HtmlEncode(v ?? "");
    }
}
