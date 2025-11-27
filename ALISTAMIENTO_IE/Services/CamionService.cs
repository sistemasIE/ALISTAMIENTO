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

        /// <summary>
        /// Obtiene un camión de la base de datos por su ID.
        /// </summary>
        /// <param name="codCamion">El código del camión a buscar.</param>
        /// <returns>Un objeto Camion si se encuentra, de lo contrario, null.</returns>
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
                // Manejo de errores. Aquí podrías registrar el error en un log
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
                    string sql = "SELECT c.* FROM CAMION c JOIN [SIE].dbo.CAMION_X_DIA cxd  ON cxd.COD_REGISTRO_CAMION  = c.COD_CAMION  WHERE cxd.COD_CAMION = @CodCamion";
                    return db.QueryFirstOrDefault<Camion>(sql, new { CodCamion = codCamion });
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores. Aquí podrías registrar el error en un log
                Console.WriteLine($"Error al obtener el camión: {ex.Message}");
                return null;
            }
        }


    }
}
