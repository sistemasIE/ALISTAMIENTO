using ALISTAMIENTO_IE.Interfaces;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class CamionXDiaService : ICamionXDiaService
    {
        private readonly string _connectionStringSIE;

        public CamionXDiaService()
        {
            // Obtiene la cadena de conexión directamente desde el App.config
            _connectionStringSIE = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        }

        // === GET ALL ===
        public async Task<List<CamionXDia>> GetAllAsync()
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                const string query = @"SELECT COD_CAMION AS CodCamion,
                                              FECHA AS Fecha,
                                              COD_EMPRESA_TRANSPORTE AS CodEmpresaTransporte,
                                              ESTADO AS Estado,
                                              COD_REGISTRO_CAMION AS CodRegistroCamion,
                                              COD_CONDUCTOR AS CodConductor
                                       FROM CAMION_X_DIA";
                var result = await connection.QueryAsync<CamionXDia>(query);
                return result.ToList();
            }
        }

        // === GET BY ID ===
        public async Task<CamionXDia> GetByIdAsync(long codCamion)
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                const string query = @"SELECT COD_CAMION AS CodCamion,
                                              FECHA AS Fecha,
                                              COD_EMPRESA_TRANSPORTE AS CodEmpresaTransporte,
                                              ESTADO AS Estado,
                                              COD_REGISTRO_CAMION AS CodRegistroCamion,
                                              COD_CONDUCTOR AS CodConductor
                                       FROM CAMION_X_DIA
                                       WHERE COD_CAMION = @CodCamion";

                return await connection.QueryFirstOrDefaultAsync<CamionXDia>(query, new { CodCamion = codCamion });
            }
        }



        public async Task<IEnumerable<CamionXDia>> ObtenerCamionesDespachadosPorFecha(DateTime fecha)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringSIE))
            {
                string sql = @"
                            SELECT 
                                COD_CAMION AS CodCamion,
                                FECHA AS Fecha,
                                COD_EMPRESA_TRANSPORTE AS CodEmpresaTransporte,
                                ESTADO AS Estado,
                                COD_REGISTRO_CAMION AS CodRegistroCamion,
                                COD_CONDUCTOR AS CodConductor
                            FROM CAMION_X_DIA cxd
                            WHERE 
                                cxd.ESTADO = 'D'
                                AND cxd.FECHA >= @Fecha
                            ORDER BY 
                                cxd.FECHA DESC;
                            ";


                var parameters = new DynamicParameters();
                parameters.Add("Fecha", fecha.Date);

                return await connection.QueryAsync<CamionXDia>(sql, parameters);
            }
        }


        // === GET BY STATUS ===
        public async Task<IEnumerable<dynamic>> GetByStatusAsync()
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                string sql = @"
            SELECT 
                cxd.COD_CAMION,
                cxd.FECHA,
                cxd.COD_EMPRESA_TRANSPORTE,
                cxd.ESTADO,
                cxd.COD_REGISTRO_CAMION,
                cxd.COD_CONDUCTOR,
                c.PLACAS AS PLACAS
            FROM CAMION_X_DIA cxd
            INNER JOIN CAMION c ON c.COD_CAMION = cxd.COD_REGISTRO_CAMION
            WHERE cxd.ESTADO = 'C';";

                return await connection.QueryAsync(sql);
            }
        }

        // === INSERT ===
        public async Task<bool> InsertAsync(CamionXDia camion)
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                const string query = @"INSERT INTO CAMION_X_DIA
                                        (COD_CAMION, FECHA, COD_EMPRESA_TRANSPORTE, ESTADO, COD_REGISTRO_CAMION, COD_CONDUCTOR)
                                       VALUES (@CodCamion, @Fecha, @CodEmpresaTransporte, @Estado, @CodRegistroCamion, @CodConductor)";

                var affectedRows = await connection.ExecuteAsync(query, camion);
                return affectedRows > 0;
            }
        }

        // === UPDATE ===
        public async Task<bool> UpdateAsync(CamionXDia camion)
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                const string query = @"UPDATE CAMION_X_DIA
                                       SET FECHA = @Fecha,
                                           COD_EMPRESA_TRANSPORTE = @CodEmpresaTransporte,
                                           ESTADO = @Estado,
                                           COD_REGISTRO_CAMION = @CodRegistroCamion,
                                           COD_CONDUCTOR = @CodConductor
                                       WHERE COD_CAMION = @CodCamion";

                var affectedRows = await connection.ExecuteAsync(query, camion);
                return affectedRows > 0;
            }
        }

        // === DELETE ===
        public async Task<bool> DeleteAsync(long codCamion)
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                const string query = @"DELETE FROM CAMION_X_DIA WHERE COD_CAMION = @CodCamion";
                var affectedRows = await connection.ExecuteAsync(query, new { CodCamion = codCamion });
                return affectedRows > 0;
            }
        }

        // === CHANGE STATUS ===
        public async Task<bool> ChangeStatusAsync(long codCamion, string newStatus)
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                const string query = @"UPDATE CAMION_X_DIA
                                       SET ESTADO = @Estado
                                       WHERE COD_CAMION = @CodCamion";

                var affectedRows = await connection.ExecuteAsync(query, new
                {
                    Estado = newStatus,
                    CodCamion = codCamion
                });

                return affectedRows > 0;
            }
        }
        public async Task<int> AnularCamionAsync(long codCamion)
        {
            using (var connection = new SqlConnection(_connectionStringSIE))
            {
                string sql = "UPDATE CAMION_X_DIA SET ESTADO = 'X' WHERE COD_CAMION = @CodCamion";
                return await connection.ExecuteAsync(sql, new { CodCamion = codCamion });
            }
        }


     
    }
}