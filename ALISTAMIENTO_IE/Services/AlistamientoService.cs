using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ALISTAMIENTO_IE.Services
{
    public class AlistamientoService
    {
        private readonly string _connectionStringSIE = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        private readonly string _connectionStringMAIN = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
        private readonly CamionXDiaService _camionXDiaService;
        private readonly DetalleCamionXDiaService _detalleCamionXDiaService;
        private readonly ItemService _itemService;

        public AlistamientoService()
        {
            _camionXDiaService = new CamionXDiaService();
            _detalleCamionXDiaService = new DetalleCamionXDiaService();
            _itemService = new ItemService();
        }
        public IEnumerable<ItemsDetalleDTO> ObtenerItemsPorAlistarCamion(int camionId)
        {
            // 1. Obtiene todos los detalles del camión.
            IEnumerable<DetalleCamionXDia> detalles = _detalleCamionXDiaService.ObtenerPorCodCamion(camionId);

            // 2. Agrupa los detalles por 'Item' y suma la cantidad planificada de cada grupo.
            var itemsAgrupados = detalles.GroupBy(x => x.Item)
                                         .Select(group => new
                                         {
                                             Item = group.Key,
                                             CantidadPlanificadaTotal = group.Sum(x => x.CANTIDAD_PLANIFICADA)
                                         });

            // 3. Obtiene las descripciones para los ítems únicos.
            List<int> itemIds = itemsAgrupados.Select(x => x.Item).ToList();
            Dictionary<int, string> descripciones = _itemService.ObtenerDescripcionesItems(itemIds, 2);

            // 4. Mapea la información agrupada al DTO final.
            List<ItemsDetalleDTO> itemsDetalles = itemsAgrupados.Select(x => new ItemsDetalleDTO
            {
                Item = x.Item,
                Descripcion = descripciones.ContainsKey(x.Item) ? descripciones[x.Item] : "Descripción no encontrada",
                CantidadPlanificada = x.CantidadPlanificadaTotal,
            }).ToList();

            return itemsDetalles;
        }


        public IEnumerable<Alistamiento> ObtenerAlistamientosActivosOrdenados()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringMAIN))
            {
                string sql = @"SELECT * FROM ALISTAMIENTO WHERE ESTADO = 'A' ORDER BY idAlistamiento DESC";
                IEnumerable<Alistamiento> alistamientos = connection.Query<Alistamiento>(sql).ToList();
                return alistamientos;
            }
        }

        public IEnumerable<DetalleCamionXDia> ObtenerDetallesPorAlistamientosActivos(DetalleCamionXDiaService detalleService)
        {
            IEnumerable<Alistamiento> alistamientos = ObtenerAlistamientosActivosOrdenados();
            List<DetalleCamionXDia> detalles = new List<DetalleCamionXDia>();
            foreach (Alistamiento alist in alistamientos)
            {
                detalles.AddRange(detalleService.ObtenerPorCodCamion(alist.IdCamionDia));
            }
            return detalles;
        }




        public IEnumerable<CamionEnAlistamientoDTO> ObtenerCamionesEnAlistamiento()
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringMAIN))
            {
                // Modificamos la consulta para incluir el estado del alistamiento con SIN_ALISTAR cuando es null
                // y agregamos ordenamiento personalizado
                string sql = @"
                   SELECT
                    cxd.COD_CAMION AS CodCamion,
                    c.PLACAS AS Placas,
                    cxd.Fecha AS FECHA,
                    cxd.ESTADO AS Estado,
                    ISNULL(a.estado, 'SIN_ALISTAR') AS EstadoAlistamiento,
                    SUM(dcxd.CANTIDAD_PLANIFICADA) AS CantTotalPedido
                    FROM [SIE].dbo.CAMION_X_DIA cxd
                    JOIN [SIE].dbo.DETALLE_CAMION_X_DIA dcxd
                        ON cxd.COD_registro_CAMION = dcxd.COD_CAMION
                    JOIN [SIE].dbo.CAMION c
                        ON c.COD_CAMION = cxd.COD_REGISTRO_CAMION 
                    LEFT JOIN ALISTAMIENTO a
                        ON a.idCamionDia = cxd.COD_CAMION
                    WHERE
                        cxd.ESTADO = 'C' 
                        AND cxd.FECHA > DATEADD(DAY, -3, GETDATE())
                        AND (a.ESTADO IS NULL OR a.ESTADO LIKE '%INCOMPLETO%' OR a.ESTADO = 'EN_PROCESO' OR a.ESTADO = 'ALISTADO' OR a.ESTADO = 'ANULADO')
                    GROUP BY
                        cxd.COD_CAMION, c.PLACAS, cxd.FECHA, cxd.ESTADO, a.estado
                    ORDER BY
                        -- Ordenamiento personalizado según prioridad: ALISTADO_INCOMPLETO, SIN_ALISTAR, EN_PROCESO, ALISTADO, ANULADO
                        CASE ISNULL(a.estado, 'SIN_ALISTAR')
                            WHEN 'ALISTADO_INCOMPLETO' THEN 1
                            WHEN 'SIN_ALISTAR' THEN 2
                            WHEN 'EN_PROCESO' THEN 3
                            WHEN 'ALISTADO' THEN 4
                            WHEN 'ANULADO' THEN 5
                            ELSE 6
                        END,
                        cxd.FECHA DESC;";

                IEnumerable<CamionEnAlistamientoDTO> result = connection.Query<CamionEnAlistamientoDTO>(sql).ToList();
                return result;
            }
        }

        public Alistamiento ObtenerAlistamientoPorCodCamionYEstado(int idCamionDia, string estado)
        {
            using (var connection = new SqlConnection(_connectionStringMAIN))
            {
                string sql = @"SELECT * FROM ALISTAMIENTO
                       WHERE idCamionDia = @idCamionDia AND estado = @estado;";

                var parameters = new { idCamionDia = idCamionDia, estado = estado };

                return connection.QueryFirstOrDefault<Alistamiento>(sql, parameters);
            }
        }

        public int InsertarAlistamiento(int idCamionDia, int idUsuario)
        {
            using (var connection = new SqlConnection(_connectionStringMAIN))
            {
                string sql = @"INSERT INTO ALISTAMIENTO (idCamionDia, idUsuario, fechaInicio, observaciones, estado)
                               VALUES (@idCamionDia, @idUsuario, @fechaInicio, @observaciones, @estado);
                               SELECT CAST(SCOPE_IDENTITY() as int);";
                var parameters = new
                {
                    idCamionDia = idCamionDia,
                    idUsuario = idUsuario,
                    fechaInicio = DateTime.Now,
                    observaciones = (string)null,
                    estado = "EN_PROCESO"
                };
                connection.Open();
                int idAlistamiento = connection.QuerySingle<int>(sql, parameters);
                return idAlistamiento;
            }
        }

        public void ActualizarAlistamiento(int idAlistamiento, string nuevoEstado, string observaciones, DateTime? fechaFin)
        {
            using (var connection = new SqlConnection(_connectionStringMAIN))
            {
                string sql = @"UPDATE ALISTAMIENTO
                               SET estado = @estado,
                                   observaciones = @observaciones,
                                   fechaFin = @fechaFin
                               WHERE idAlistamiento = @idAlistamiento";
                connection.Execute(sql, new
                {
                    estado = nuevoEstado,
                    observaciones = observaciones,
                    fechaFin = fechaFin,
                    idAlistamiento = idAlistamiento
                });
            }
        }
    }
}
