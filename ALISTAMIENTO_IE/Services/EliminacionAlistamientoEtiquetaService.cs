using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    internal class EliminacionAlistamientoEtiquetaService : IEliminacionAlistamientoEtiquetaService
    {
        private readonly string _connectionString;

        public EliminacionAlistamientoEtiquetaService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
        }

        public async Task<int> InsertarEliminacionesAsync(IEnumerable<EliminacionAlistamientoEtiqueta> registros)
        {
            const string sql = @"INSERT INTO ELIMINADAS_ALISTAMIENTO_ETIQUETA
                (idAlistamientoEtiqueta, fechaEliminacion, idUsuarioElimina, observaciones)
                VALUES (@IdAlistamientoEtiqueta, @FechaEliminacion, @IdUsuarioElimina, @Observaciones);";

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            using var tx = connection.BeginTransaction();
            try
            {
                int total = 0;
                foreach (var reg in registros)
                {
                    // Ignoramos el IdEliminacionAlistamientoEtiqueta porque es IDENTITY.
                    total += await connection.ExecuteAsync(sql, new
                    {
                        reg.IdAlistamientoEtiqueta,
                        FechaEliminacion = reg.FechaEliminacion == default ? DateTime.Now : reg.FechaEliminacion,
                        reg.IdUsuarioElimina,
                        reg.Observaciones
                    }, tx);
                }
                tx.Commit();
                return total;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public async Task<int> InsertarEliminacionAsync(int idAlistamientoEtiqueta, int idUsuarioElimina, string? observaciones)
        {
            const string sql = @"INSERT INTO ELIMINADAS_ALISTAMIENTO_ETIQUETA
                (idAlistamientoEtiqueta, idUsuarioElimina, observaciones)
                VALUES (@IdAlistamientoEtiqueta, @IdUsuarioElimina, @Observaciones);";

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, new
            {
                IdAlistamientoEtiqueta = idAlistamientoEtiqueta,
                IdUsuarioElimina = idUsuarioElimina,
                Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim()
            });
        }

        public async Task<int> EliminarRegistroAsync(int idEliminacionAlistamientoEtiqueta)
        {
            const string sql = @"DELETE FROM ELIMINADAS_ALISTAMIENTO_ETIQUETA WHERE idEliminacionAlistamientoEtiqueta = @Id;";
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, new { Id = idEliminacionAlistamientoEtiqueta });
        }

        public async Task<IEnumerable<EliminacionAlistamientoEtiqueta>> ObtenerEliminacionesPorAlistamientoEtiquetaAsync(int idAlistamientoEtiqueta)
        {
            const string sql = @"SELECT idEliminacionAlistamientoEtiqueta AS IdEliminacionAlistamientoEtiqueta,
                                        idAlistamientoEtiqueta AS IdAlistamientoEtiqueta,
                                        fechaEliminacion AS FechaEliminacion,
                                        idUsuarioElimina AS IdUsuarioElimina,
                                        observaciones AS Observaciones
                                   FROM ELIMINADAS_ALISTAMIENTO_ETIQUETA
                                   WHERE idAlistamientoEtiqueta = @Id;";
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<EliminacionAlistamientoEtiqueta>(sql, new { Id = idAlistamientoEtiqueta });
        }

        public async Task<IEnumerable<EliminacionAlistamientoEtiqueta>> ObtenerEliminacionesPorAlistamientoAsync(int idAlistamiento)
        {
            const string sql = @"SELECT eae.idEliminacionAlistamientoEtiqueta AS IdEliminacionAlistamientoEtiqueta,
                                        eae.idAlistamientoEtiqueta AS IdAlistamientoEtiqueta,
                                        eae.fechaEliminacion AS FechaEliminacion,
                                        eae.idUsuarioElimina AS IdUsuarioElimina,
                                        eae.observaciones AS Observaciones
                                   FROM ELIMINADAS_ALISTAMIENTO_ETIQUETA eae
                                   JOIN ALISTAMIENTO_ETIQUETA ae ON eae.idAlistamientoEtiqueta = ae.idAlistamientoEtiqueta
                                   WHERE ae.idAlistamiento = @IdAlistamiento;";
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return await connection.QueryAsync<EliminacionAlistamientoEtiqueta>(sql, new { IdAlistamiento = idAlistamiento });
        }
    }
}
