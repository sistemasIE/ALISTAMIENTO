using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class CamionXDiaService
    {
        private readonly string _connectionStringSIE;

        public CamionXDiaService()
        {
            // Obtiene la cadena de conexión directamente desde el App.config
            _connectionStringSIE = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        }



    }
}