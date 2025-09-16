using LECTURA_DE_BANDA;

namespace ALISTAMIENTO_IE
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

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