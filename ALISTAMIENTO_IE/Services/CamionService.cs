using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ALISTAMIENTO_IE.Services
{
    /// <summary>
    /// Servicio para la capa de acceso a datos de los camiones.
    /// Utiliza Dapper para consultar la base de datos de manera eficiente.
    /// </summary>
    public class CamionService : ICamionService
    {
        private readonly string _connectionString;

        public CamionService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        }

        #region Métodos Existentes (Backwards Compatibility)

        /// <summary>
        /// Obtiene un camión de la base de datos por su ID.
        /// </summary>
        public Camion? GetCamionById(int codCamion)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT * FROM CAMION WHERE COD_CAMION = @CodCamion";
                    return db.QueryFirstOrDefault<Camion>(sql, new { CodCamion = codCamion });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el camión: {ex.Message}");
                return null;
            }
        }

        public Camion? GetCamionByCamionXDiaId(int codCamion)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    string sql = "SELECT c.* FROM CAMION c JOIN [SIE].dbo.CAMION_X_DIA cxd ON cxd.COD_REGISTRO_CAMION = c.COD_CAMION WHERE cxd.COD_CAMION = @CodCamion";
                    return db.QueryFirstOrDefault<Camion>(sql, new { CodCamion = codCamion });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener el camión: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Métodos CRUD Async

        /// <summary>
        /// Obtiene todos los camiones
        /// </summary>
        public async Task<IEnumerable<Camion>> ObtenerTodosAsync()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COD_CAMION, PLACAS, TIPOLOGIA FROM CAMION ORDER BY PLACAS";
                return await db.QueryAsync<Camion>(sql);
            }
        }

        /// <summary>
        /// Obtiene un camión por su código
        /// </summary>
        public async Task<Camion?> ObtenerPorIdAsync(long codCamion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COD_CAMION, PLACAS, TIPOLOGIA FROM CAMION WHERE COD_CAMION = @CodCamion";
                return await db.QueryFirstOrDefaultAsync<Camion>(sql, new { CodCamion = codCamion });
            }
        }

        /// <summary>
        /// Busca camiones por placas (búsqueda parcial)
        /// </summary>
        public async Task<IEnumerable<Camion>> BuscarPorPlacasAsync(string placas)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COD_CAMION, PLACAS, TIPOLOGIA FROM CAMION WHERE PLACAS LIKE @Placas ORDER BY PLACAS";
                return await db.QueryAsync<Camion>(sql, new { Placas = $"%{placas}%" });
            }
        }

        /// <summary>
        /// Inserta un nuevo camión
        /// </summary>
        public async Task<long> InsertarAsync(Camion camion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                // Obtener el siguiente código
                string sqlMax = "SELECT ISNULL(MAX(COD_CAMION), 0) + 1 FROM CAMION";
                long nuevoCodigo = await db.ExecuteScalarAsync<long>(sqlMax);

                string sql = @"
                    INSERT INTO CAMION (COD_CAMION, PLACAS, TIPOLOGIA)
                    VALUES (@COD_CAMION, @PLACAS, @TIPOLOGIA)";

                camion.COD_CAMION = nuevoCodigo;
                await db.ExecuteAsync(sql, camion);

                return nuevoCodigo;
            }
        }

        /// <summary>
        /// Actualiza un camión existente
        /// </summary>
        public async Task<bool> ActualizarAsync(Camion camion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = @"
                    UPDATE CAMION
                    SET PLACAS = @PLACAS,
                        TIPOLOGIA = @TIPOLOGIA
                    WHERE COD_CAMION = @COD_CAMION";

                int filasAfectadas = await db.ExecuteAsync(sql, camion);
                return filasAfectadas > 0;
            }
        }

        /// <summary>
        /// Elimina un camión por su código
        /// </summary>
        public async Task<bool> EliminarAsync(long codCamion)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "DELETE FROM CAMION WHERE COD_CAMION = @CodCamion";
                int filasAfectadas = await db.ExecuteAsync(sql, new { CodCamion = codCamion });
                return filasAfectadas > 0;
            }
        }

        /// <summary>
        /// Verifica si existe un camión con las placas especificadas
        /// </summary>
        public async Task<bool> ExistePlacasAsync(string placas, long? codCamionExcluir = null)
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string sql = "SELECT COUNT(1) FROM CAMION WHERE PLACAS = @Placas";

                if (codCamionExcluir.HasValue)
                {
                    sql += " AND COD_CAMION != @CodCamionExcluir";
                }

                int count = await db.ExecuteScalarAsync<int>(sql, new { Placas = placas, CodCamionExcluir = codCamionExcluir });
                return count > 0;
            }
        }

        #endregion
    }
}
