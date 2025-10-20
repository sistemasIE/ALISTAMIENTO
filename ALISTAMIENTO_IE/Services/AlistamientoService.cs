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

        public async Task<IEnumerable<CamionItemsDto>> ObtenerItemsPorAlistarCamion(int camionId)
        {

            using (var connection = new SqlConnection(_connectionStringMAIN))
            {
                string sql = @"
                    SELECT
                    c.PLACAS AS Placas,
                    cxd.fecha AS Fecha,
                    i.f120_descripcion AS Descripcion,
                    dcxd.item AS Item,
                    i.f120_id_unidad_inventario as UNIDAD,
                    i.f120_id_unidad_empaque as EMB,
                    SUM(dcxd.CANTIDAD_PLANIFICADA) AS CantTotalPedido,
                    CASE 
                        WHEN i.f120_id_unidad_inventario = 'UND' 
                        THEN SUM(dcxd.CANTIDAD_PLANIFICADA) / 
                             TRY_CAST(SUBSTRING(i.f120_id_unidad_empaque, 2, LEN(i.f120_id_unidad_empaque)) AS INT)
                        ELSE 0
                    END AS PacasEsperadas,
                    CASE 
                        WHEN i.f120_id_unidad_inventario = 'KLS'
                        THEN SUM(dcxd.CANTIDAD_PLANIFICADA)
                        ELSE 0
                    END AS KilosEsperados
                FROM [SIE].dbo.CAMION_X_DIA cxd
                JOIN [SIE].dbo.DETALLE_CAMION_X_DIA dcxd
                    ON cxd.COD_CAMION = dcxd.COD_CAMION
                JOIN [SIE].dbo.CAMION c
                    ON c.COD_CAMION = cxd.COD_REGISTRO_CAMION 
                LEFT JOIN ALISTAMIENTO a
                    ON a.idCamionDia = cxd.COD_CAMION
                LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items i
                    ON dcxd.ITEM = i.f120_id
                WHERE
                    cxd.COD_CAMION = @idCamionDia AND i.f120_id_cia = 2
                GROUP BY
                    cxd.COD_CAMION, c.PLACAS, cxd.FECHA, i.f120_id_unidad_inventario, i.f120_id, i.f120_descripcion,
                    dcxd.item, cxd.ESTADO, a.estado, i.f120_id_unidad_empaque, dcxd.PTO_ENVIO
                ORDER BY
                    CantTotalPedido DESC;
                ";


                var result = await connection.QueryAsync<CamionItemsDto>(sql, new { idCamionDia = camionId });

                return result.ToList();
            }

        }

        /// <summary>
        /// Calcula los totales de un conjunto de ítems de camión
        /// </summary>
        /// <param name="items">Colección de ítems del camión</param>
        /// <returns>DTO con los totales calculados</returns>
        public ReporteImpresionTotalesDto CalcularTotalesReporte(IEnumerable<CamionItemsDto> items)
        {
            return new ReporteImpresionTotalesDto
            {
                TotalCantTotalPedido = items.Sum(x => x.CantTotalPedido),
                TotalPacasEsperadas = items.Sum(x => x.PacasEsperadas ?? 0),
                TotalKilosEsperados = items.Sum(x => x.KilosEsperados ?? 0)
            };
        }

        /// <summary>
        /// Agrega una fila de totales a un DataTable con los valores calculados
        /// </summary>
        /// <param name="dataTable">DataTable al que se agregará la fila de totales</param>
        /// <param name="totales">Objeto con los totales calculados</param>
        public void AgregarFilaTotalesADataTable(DataTable dataTable, ReporteImpresionTotalesDto totales)
        {
            if (dataTable == null || dataTable.Rows.Count == 0)
                return;

            DataRow totalRow = dataTable.NewRow();

            // Asignar valores a las columnas numéricas si existen
            if (dataTable.Columns.Contains("CantTotalPedido"))
                totalRow["CantTotalPedido"] = totales.TotalCantTotalPedido;

            if (dataTable.Columns.Contains("PacasEsperadas"))
                totalRow["PacasEsperadas"] = totales.TotalPacasEsperadas;

            if (dataTable.Columns.Contains("KilosEsperados"))
                totalRow["KilosEsperados"] = totales.TotalKilosEsperados;

            // Buscar una columna de tipo string para poner el label "TOTALES"
            bool totalLabelSet = false;

            // Primero intentar con columnas conocidas
            string[] possibleColumns = { "Item", "Descripcion", "Descripción", "Nombre", "Producto" };
            foreach (var colName in possibleColumns)
            {
                if (dataTable.Columns.Contains(colName) && dataTable.Columns[colName].DataType == typeof(string))
                {
                    totalRow[colName] = "TOTALES";
                    totalLabelSet = true;
                    break;
                }
            }

            // Si no se encontró, buscar la primera columna de tipo string
            if (!totalLabelSet)
            {
                foreach (DataColumn col in dataTable.Columns)
                {
                    if (col.DataType == typeof(string))
                    {
                        totalRow[col.ColumnName] = "TOTALES";
                        break;
                    }
                }
            }

            // Agregar la fila de totales al final del DataTable
            dataTable.Rows.Add(totalRow);
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
                string sql = @"
                   SELECT
                     cxd.COD_CAMION AS CodCamion,
                     c.PLACAS AS Placas,
                     cxd.fecha as FECHA,
                     cxd.ESTADO AS Estado,
                     ISNULL(a.estado, 'SIN_ALISTAR') AS EstadoAlistamiento,
                     sum(dcxd.CANTIDAD_PLANIFICADA ) as cantTotalPedido
                     FROM [SIE].dbo.CAMION_X_DIA cxd
                     JOIN [SIE].dbo.DETALLE_CAMION_X_DIA dcxd
                         ON cxd.COD_CAMION = dcxd.COD_CAMION
                     JOIN [SIE].dbo.CAMION c
                         ON c.COD_CAMION = cxd.COD_REGISTRO_CAMION 
                     LEFT JOIN ALISTAMIENTO a
                         ON a.idCamionDia = cxd.COD_CAMION
                     WHERE
 	                     cxd.ESTADO = 'C' -- Sin alistar
	                     AND cxd.FECHA > DATEADD(DAY, -7, GETDATE())
                         AND (a.ESTADO IS NULL OR a.ESTADO LIKE '%INCOMPLETO%' OR a.ESTADO = 'EN_PROCESO' OR a.ESTADO = 'ALISTADO' OR a.ESTADO = 'ANULADO')
                    GROUP BY
                        cxd.COD_CAMION, c.PLACAS, cxd.FECHA, cxd.ESTADO, a.estado 
                    ORDER BY
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
