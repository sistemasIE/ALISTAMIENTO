using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Services;
using LECTURA_DE_BANDA;
using Microsoft.Extensions.DependencyInjection;
using QuestPDF.Infrastructure;
namespace ALISTAMIENTO_IE
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            var serviceProvider = new ServiceCollection()
            .AddTransient<IAlistamientoEtiquetaService, AlistamientoEtiquetaService>()
            .AddTransient<IAlistamientoService, AlistamientoService>()
            .BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            Application.SetDefaultFont(new Font("Segoe UI", 11F));

            QuestPDF.Settings.License = LicenseType.Community;

            using (var loginForm = new Login())
            {
                if (loginForm.ShowDialog() == DialogResult.OK && loginForm.UsuarioAutenticado != null)
                {
                    Application.Run(new Menu());
                }
            }
        }
    }
}