using LECTURA_DE_BANDA;
using QuestPDF.Infrastructure;
namespace ALISTAMIENTO_IE
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            QuestPDF.Settings.License = LicenseType.Community;

            using (var loginForm = new Login())
            {
                if (loginForm.ShowDialog() == DialogResult.OK && loginForm.UsuarioAutenticado != null)
                {
                    Application.Run(new ALISTAR_CAMION());
                }
            }
        }
    }
}