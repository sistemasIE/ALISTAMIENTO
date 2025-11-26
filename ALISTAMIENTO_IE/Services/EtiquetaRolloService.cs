using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class EtiquetaRolloService : IEtiquetaRolloService
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

            const string sql = @"
                SELECT 
                    COD_ETIQUETA_ROLLO AS CodEtiquetaRollo,
                    COD_BARRAS AS CodBarras,
                    FECHA AS Fecha,
                    ITEM AS Item,
                    TELAR AS Telar,
                    TEJEDOR AS Tejedor,
                    PESO_BRUTO AS PesoBruto,
                    PESO_NETO AS PesoNeto,
                    ESTADO AS Estado,
                    COD_TIPO_ETIQUETADO AS CodTipoEtiquetado,
                    METROS AS Metros,
                    ESTADO_PA_IPT AS EstadoPaIpt,
                    CI_OPERADOR AS CiOperador,
                    TURNO AS Turno,
                    CONSUMIDA_EN AS ConsumidaEn,
                    DESPACHADA_EN AS DespachadaEn
                FROM ETIQUETA_ROLLO 
                WHERE COD_ETIQUETA_ROLLO = @codigo";

            return await connection.QueryFirstOrDefaultAsync<EtiquetaRollo>(sql, new { codigo = codigoEtiqueta });
        }
    }
}
