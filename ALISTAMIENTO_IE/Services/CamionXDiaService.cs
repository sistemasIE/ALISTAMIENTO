using Dapper;
using Microsoft.Data.SqlClient;
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


        public IEnumerable<int> ObtenerCodCamionesPorEstadoYFechas(string estado)
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                string sql = @"SELECT DISTINCT COD_CAMION_INT FROM SIE.DETALLE_CAMION_X_DIA 
                                WHERE ESTADO = @estado 
                                AND CAST(FECHA AS DATE) >= CAST(DATEADD(day, -1, GETDATE()) AS DATE)
                                AND CAST(FECHA AS DATE) <= CAST(GETDATE() AS DATE)";
                return connection.Query<int>(sql, new { estado }).ToList();
            }
        }
    }
}