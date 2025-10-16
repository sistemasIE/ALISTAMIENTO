using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class EtiquetaRolloService
    {
        private readonly string _connectionString;

        public EtiquetaRolloService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
        }

        public async Task<EtiquetaRollo?> ObtenerEtiquetaRolloPorCodigoAsync(string codigoEtiqueta)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"SELECT 
                CodEtiquetaRollo,
                CodBarras,
                Fecha,
                Item,
                Telar,
                Tejedor,
                PesoBruto,
                PesoNeto,
                Estado,
                CodTipoEtiquetado,
                Metros,
                EstadoPaIpt,
                CiOperador,
                Turno,
                ConsumidaEn,
                DespachadaEn
            FROM ETIQUETA_ROLLO 
            WHERE CodEtiquetaRollo = @codigo";

            return await connection.QueryFirstOrDefaultAsync<EtiquetaRollo>(sql, new { codigo = codigoEtiqueta });
        }
    }
}
