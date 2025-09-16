using ALISTAMIENTO_IE.Model;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class EtiquetaLinerService
    {
        private readonly string _connectionString;

        public EtiquetaLinerService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
        }

        public async Task<EtiquetaLiner?> ObtenerEtiquetaLinerPorCodigoAsync(string codigoEtiqueta)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"SELECT * FROM ETIQUETA_LINER WHERE COD_ETIQUETA_LINER = @codigo";

            return await connection.QueryFirstOrDefaultAsync<EtiquetaLiner>(sql, new { codigo = codigoEtiqueta });
        }

    }
}
