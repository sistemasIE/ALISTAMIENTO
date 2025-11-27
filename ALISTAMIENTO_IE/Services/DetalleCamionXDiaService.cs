using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class DetalleCamionXDiaService : IDetalleCamionXDiaService
    {
        private readonly string _connectionString;

        public DetalleCamionXDiaService() // Si NO usas un Contenedor de DI (WinForms puro y ConfigurationManager)
        {
            // Usar la asignación estática solo si el ambiente NO tiene DI
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        }


        // SELECT por COD_CAMION (ya existente, actualizado para evitar SELECT *)
        public IEnumerable<DetalleCamionXDia> ObtenerPorCodCamion(int codCamion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT 
                        COD_DETALLE_CAMION AS CodDetalleCamion,
                        COD_CAMION AS CodCamion,
                        ITEM,
                        ITEM_EQUIVALENTE AS ItemEquivalente,
                        ESTADO,
                        CANTIDAD_PLANIFICADA
                    FROM DETALLE_CAMION_X_DIA
                    WHERE COD_CAMION = @codCamion";

                return connection.Query<DetalleCamionXDia>(sql, new { codCamion }).ToList();
            }
        }

        // SELECT por COD_CAMION y ITEM
        public IEnumerable<DetalleCamionXDia> ObtenerPorCodCamionYItem(int codCamion, int item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    SELECT 
                        COD_DETALLE_CAMION AS CodDetalleCamion,
                        COD_CAMION AS CodCamion,
                        ITEM,
                        ITEM_EQUIVALENTE AS ItemEquivalente,
                        ESTADO,
                        CANTIDAD_PLANIFICADA
                    FROM DETALLE_CAMION_X_DIA
                    WHERE COD_CAMION = @codCamion AND ITEM = @item";

                return connection.Query<DetalleCamionXDia>(sql, new { codCamion, item }).ToList();
            }
        }

        // UPDATE por COD_CAMION y ITEM
        public int ActualizarItemDadoCodCamionEItem(int codCamion, int item, int nuevoItem)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                    UPDATE DETALLE_CAMION_X_DIA
                    SET 
                        ITEM = @nuevoItem
                    WHERE COD_CAMION = @codCamion AND ITEM = @item";

                return connection.Execute(sql, new
                {
                    codCamion,
                    item,
                    nuevoItem
                });
            }
        }

    }
}