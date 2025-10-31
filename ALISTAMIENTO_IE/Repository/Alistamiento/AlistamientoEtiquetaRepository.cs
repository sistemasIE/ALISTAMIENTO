using ALISTAMIENTO_IE.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Repository.Alistamiento
{
    public class AlistamientoEtiquetaRepository : IAlistamientoEtiquetaRepository
    {

        private readonly string _connectionStringSIE = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        private readonly string _connectionStringMAIN = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;


        public async Task<AlistamientoDetalleDto> ObtenerPorAlistamientoAsync(int idCamionDia)
        {
            using var connection = new SqlConnection(_connectionStringMAIN);

            string sql = @"
        SELECT a.*, ae.*
        FROM ALISTAMIENTO a
        LEFT JOIN ALISTAMIENTO_ETIQUETA ae ON a.IdAlistamiento = ae.IdAlistamiento
        WHERE a.IdCamionDia = @idCamionDia
        ORDER BY a.IdAlistamiento;";

            var alistamientoDict = new Dictionary<int, AlistamientoDetalleDto>();

            var result = await connection.QueryAsync<AlistamientoDetalleDto, AlistamientoEtiqueta, AlistamientoDetalleDto>(
                sql,
                (alistamiento, etiqueta) =>
                {
                    if (!alistamientoDict.TryGetValue(alistamiento.IdAlistamiento, out var current))
                    {
                        current = alistamiento;
                        current.Etiquetas = new List<AlistamientoEtiqueta>();
                        alistamientoDict.Add(current.IdAlistamiento, current);
                    }

                    if (etiqueta != null)
                        current.Etiquetas.Add(etiqueta);

                    return current;
                },
                new { idCamionDia },
                splitOn: "IdAlistamiento"
            );

            return alistamientoDict.Values.FirstOrDefault();
        }

        public async Task<List<AlistamientoItemDTO>> GetItemsAlistadosAsync(int idCamionDia)
        {
            var query = @"
        SELECT 
            COALESCE(e.cod_item, '') AS item,
            CASE 
                WHEN e.cod_item LIKE 'S%' THEN 'SACOS'
                WHEN e.cod_item LIKE 'L%' THEN 'LINER'
                WHEN e.cod_item LIKE 'R%' THEN 'ROLLO'
                ELSE 'DESCONOCIDO'
            END AS tipoProducto,
            SUM(e.cantidad) AS total
        FROM etiqueta e
        WHERE e.cod_etiqueta IN (
            SELECT ae.etiqueta
            FROM ALISTAMIENTO_ETIQUETA ae
            JOIN ALISTAMIENTO a ON a.idAlistamiento = ae.idAlistamiento
            WHERE a.idCamionDia = @idCodCamionDia
        )
        GROUP BY e.cod_item
    ";

            using (var connection = new SqlConnection(_connectionStringMAIN))
            {
                var result = await connection.QueryAsync<AlistamientoItemDTO>(
                    query, new { idCodCamionDia = idCamionDia });
                return result.ToList();
            }
        }

    }




}
