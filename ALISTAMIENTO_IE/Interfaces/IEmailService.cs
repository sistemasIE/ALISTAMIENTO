namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(string asunto, string cuerpoHtml, string[] destinatarios);

        Task NotificarAnulacionCamionAsync(long codCamion, string texto, string causa, string[] destinatarios);
    }

}
