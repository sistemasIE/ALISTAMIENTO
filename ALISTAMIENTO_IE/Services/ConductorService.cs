using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ALISTAMIENTO_IE.Services
{
    /// <summary>
    /// Servicio para la capa de acceso a datos de los conductores.
    /// Utiliza Dapper para consultar la base de datos de manera eficiente.
    /// </summary>
    public class ConductorService : IConductorService
    {
        private readonly string _connectionString;

        public ConductorService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        }

        #region Métodos CRUD Async

        /// <summary>
        /// Obtiene todos los conductores
        /// </summary>
        public async Task<IEnumerable<Conductor>> ObtenerTodosAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COD_CONDUCTOR, NOMBRES, TELEFONO, CI FROM CONDUCTOR ORDER BY NOMBRES";
                return await db.QueryAsync<Conductor>(sql);
            }
        }

        /// <summary>
        /// Obtiene un conductor por su código
        /// </summary>
        public async Task<Conductor?> ObtenerPorIdAsync(long codConductor)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COD_CONDUCTOR, NOMBRES, TELEFONO, CI FROM CONDUCTOR WHERE COD_CONDUCTOR = @CodConductor";
                return await db.QueryFirstOrDefaultAsync<Conductor>(sql, new { CodConductor = codConductor });
            }
        }

        /// <summary>
        /// Busca conductores por nombre (búsqueda parcial)
        /// </summary>
        public async Task<IEnumerable<Conductor>> BuscarPorNombreAsync(string nombre)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COD_CONDUCTOR, NOMBRES, TELEFONO, CI FROM CONDUCTOR WHERE NOMBRES LIKE @Nombre ORDER BY NOMBRES";
                return await db.QueryAsync<Conductor>(sql, new { Nombre = $"%{nombre}%" });
            }
        }

        /// <summary>
        /// Busca un conductor por su cédula de identidad
        /// </summary>
        public async Task<Conductor?> BuscarPorCIAsync(string ci)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COD_CONDUCTOR, NOMBRES, TELEFONO, CI FROM CONDUCTOR WHERE CI = @CI";
                return await db.QueryFirstOrDefaultAsync<Conductor>(sql, new { CI = ci });
            }
        }

        /// <summary>
        /// Inserta un nuevo conductor
        /// </summary>
        public async Task<long> InsertarAsync(Conductor conductor)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // Obtener el siguiente código
                string sqlMax = "SELECT ISNULL(MAX(COD_CONDUCTOR), 0) + 1 FROM CONDUCTOR";
                long nuevoCodigo = await db.ExecuteScalarAsync<long>(sqlMax);

                string sql = @"
                    INSERT INTO CONDUCTOR (COD_CONDUCTOR, NOMBRES, TELEFONO, CI)
                    VALUES (@COD_CONDUCTOR, @NOMBRES, @TELEFONO, @CI)";

                conductor.COD_CONDUCTOR = nuevoCodigo;
                await db.ExecuteAsync(sql, conductor);

                return nuevoCodigo;
            }
        }

        /// <summary>
        /// Actualiza un conductor existente
        /// </summary>
        public async Task<bool> ActualizarAsync(Conductor conductor)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = @"
                    UPDATE CONDUCTOR
                    SET NOMBRES = @NOMBRES,
                        TELEFONO = @TELEFONO,
                        CI = @CI
                    WHERE COD_CONDUCTOR = @COD_CONDUCTOR";

                int filasAfectadas = await db.ExecuteAsync(sql, conductor);
                return filasAfectadas > 0;
            }
        }

        /// <summary>
        /// Elimina un conductor por su código
        /// </summary>
        public async Task<bool> EliminarAsync(long codConductor)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM CONDUCTOR WHERE COD_CONDUCTOR = @CodConductor";
                int filasAfectadas = await db.ExecuteAsync(sql, new { CodConductor = codConductor });
                return filasAfectadas > 0;
            }
        }

        /// <summary>
        /// Verifica si existe un conductor con la cédula especificada
        /// </summary>
        public async Task<bool> ExisteCIAsync(string ci, long? codConductorExcluir = null)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM CONDUCTOR WHERE CI = @CI";

                if (codConductorExcluir.HasValue)
                {
                    sql += " AND COD_CONDUCTOR != @CodConductorExcluir";
                }

                int count = await db.ExecuteScalarAsync<int>(sql, new { CI = ci, CodConductorExcluir = codConductorExcluir });
                return count > 0;
            }
        }

        #endregion
    }
}
