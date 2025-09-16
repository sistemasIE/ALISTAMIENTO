using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class DetalleCamionXDiaService
    {
        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;

        public IEnumerable<DetalleCamionXDia> ObtenerPorCodCamion(int codCamion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"SELECT * FROM DETALLE_CAMION_X_DIA WHERE COD_CAMION = @codCamion";
                return connection.Query<DetalleCamionXDia>(sql, new { codCamion }).ToList();
            }
        }
    }
}
