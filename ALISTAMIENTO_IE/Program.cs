// Program.cs
using ALISTAMIENTO_IE.Forms;
using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Services;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
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

                // --- REGISTRO DE SERVICIOS CON INTERFAZ ---
                .AddTransient<IAlistamientoService, AlistamientoService>()
                .AddTransient<IAlistamientoEtiquetaService, AlistamientoEtiquetaService>()
                .AddTransient<IKardexService, KardexService>()
                .AddTransient<IEliminacionAlistamientoEtiquetaService, EliminacionAlistamientoEtiquetaService>()
                .AddTransient<IPdfService, QuestPDFService>() // Asumido
                .AddTransient<IDataGridViewExporter, DataGridViewExporter>() // Asumido
                .AddTransient<ICamionXDiaService, CamionXDiaService>()
                .AddTransient<IConductorService, ConductorService>()
                .AddTransient<IDetalleCamionXDiaService, DetalleCamionXDiaService>()
                .AddTransient<IEmailService, EmailService>()
                .AddTransient<IItemService, ItemService>()

                // Otros servicios necesarios para ALISTAMIENTO (aunque no se usen directo en Menu)
                .AddTransient<ICamionService, CamionService>()

                .AddTransient<IAuthorizationService>(sp => AuthorizationService.CreateFromConfig("stringConexionLocal"))
                .AddTransient<IEtiquetaService, EtiquetaService>()
                .AddTransient<IEtiquetaLinerService, EtiquetaLinerService>()
                .AddTransient<IEtiquetaRolloService, EtiquetaRolloService>()

                // --- SERVICIOS SIN INTERFAZ (Se inyectan directamente por tipo) ---
                .AddTransient<CargueMasivoService>()

                // --- REGISTRO DE FORMS (Se deben crear por el contenedor) ---
                .AddTransient<EtqsEliminadasEnAlistamiento>()
                .AddTransient<ALISTAMIENTO>()
                .AddTransient<Menu>()
                .AddTransient<EtqsEliminadasEnAlistamiento>()
                .AddTransient<ALISTAMIENTO>()
                .AddTransient<FormOperarEtiquetas>()
                .AddTransient<CONSULTA_ITEMS_ETIQUETAS>()
                .AddTransient<OBSERVACIONES>()
                .AddTransient<Menu>()

                .BuildServiceProvider();

            ApplicationConfiguration.Initialize();
            Application.SetDefaultFont(new Font("Segoe UI", 11F));

            QuestPDF.Settings.License = LicenseType.Community;

            using (var loginForm = new Login())
            {
                if (loginForm.ShowDialog() == DialogResult.OK && loginForm.UsuarioAutenticado != null)
                {
                    Application.Run(serviceProvider.GetRequiredService<Menu>());
                }
            }
        }
    }
}