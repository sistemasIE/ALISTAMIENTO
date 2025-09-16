using ALISTAMIENTO_IE.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    // DTO para manejar los totales, según la definición en el formulario.
    public class ReporteTotalesDto
    {
        public int TotalPacas { get; set; }
        public int TotalCamiones { get; set; }
    }
    internal class AlistamientoEtiquetaService
    {
        private readonly string _connectionString;

        public AlistamientoEtiquetaService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
        }

        /// <summary>
        /// Calcula el rango de fechas para una ventana de 24 horas (7:00 AM a 7:00 AM del día siguiente).
        /// </summary>
        /// <param name="fechaConsulta">Fecha de referencia para el cálculo.</param>
        /// <returns>Una tupla con la fecha de inicio y la fecha de fin del turno.</returns>
        private (DateTime fechaInicio, DateTime fechaFin) ObtenerRangoFechas(DateTime fechaConsulta)
        {
            DateTime fechaInicio;
            DateTime fechaFin;

            if (fechaConsulta.Hour < 7)
            {
                fechaFin = fechaConsulta.Date.AddHours(7);
                fechaInicio = fechaFin.AddDays(-1);
            }
            else
            {
                fechaInicio = fechaConsulta.Date.AddHours(7);
                fechaFin = fechaInicio.AddDays(1);
            }

            return (fechaInicio, fechaFin);
        }

        public async Task<IEnumerable<dynamic>> ObtenerReportePacasPorHora(DateTime fechaConsulta)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Usamos la función privada para obtener el rango de fechas.
                var (fechaInicio, fechaFin) = ObtenerRangoFechas(fechaConsulta);

                string sql = @"
                    SELECT 
                        u.loginNombre AS USUARIO,
                        ROUND(CAST(COUNT(ae.etiqueta) AS DECIMAL(10, 2)) / 8,0) AS ""PACAS / H"",
                        COUNT(DISTINCT d.COD_CAMION) AS ""CARROS DESPACHADOS""
                    FROM 
                        ALISTAMIENTO a
                    JOIN 
                        USUARIOS u ON u.UsuarioID = a.idUsuario 
                    JOIN 
                        ALISTAMIENTO_ETIQUETA ae ON ae.idAlistamiento = a.idAlistamiento
                    LEFT JOIN 
                        etiqueta e ON e.COD_ETIQUETA = ae.etiqueta
                    LEFT JOIN 
                        ETIQUETA_LINER el ON el.COD_ETIQUETA_LINER = ae.etiqueta
                    LEFT JOIN
                        [SIE].dbo.DETALLE_CAMION_X_DIA d ON a.idCamionDia = d.COD_CAMION
                    WHERE 
                        ae.fecha >= @fechaInicio
                        AND ae.fecha < @fechaFin
                        AND a.estado LIKE '%alistado%'
                    GROUP BY 
                        u.loginNombre;";

                return await connection.QueryAsync(sql, new { fechaInicio, fechaFin });
            }
        }




        public async Task<IEnumerable<dynamic>> ObtenerReportePacasPorHoraPorTurno(DateTime fechaConsulta, string? turnoLike)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Usamos la función privada para obtener el rango de fechas.
                var (fechaInicio, fechaFin) = ObtenerRangoFechas(fechaConsulta);

                var sql = @"
                    SELECT 
                        u.loginNombre AS USUARIO,
                        c.PLACAS AS CAMION,
                        ROUND(CAST(COUNT(ae.etiqueta) AS DECIMAL(10, 2)) / 8, 0) AS ""PACAS/H"",
                        SUM(ISNULL(e.CANTIDAD, el.Cantidad)) AS ""CANT. SACOS""
                    FROM 
                        ALISTAMIENTO a
                    JOIN 
                        USUARIOS u ON u.UsuarioID = a.idUsuario 
                    JOIN 
                        ALISTAMIENTO_ETIQUETA ae ON ae.idAlistamiento = a.idAlistamiento
                    LEFT JOIN 
                        etiqueta e ON e.COD_ETIQUETA = ae.etiqueta
                    LEFT JOIN 
                        ETIQUETA_LINER el ON el.COD_ETIQUETA_LINER = ae.etiqueta
                    LEFT JOIN
                        [SIE].dbo.CAMION_X_DIA cxd ON a.idCamionDia = cxd.COD_CAMION
                    LEFT JOIN
                        [SIE].dbo.CAMION c ON cxd.COD_REGISTRO_CAMION = c.COD_CAMION
                    WHERE 
                        ae.fecha >= @fechaInicio
                        AND ae.fecha < @fechaFin
                        AND a.estado LIKE '%alistado%'";

                string? likeParam = null;
                if (!string.IsNullOrWhiteSpace(turnoLike))
                {
                    likeParam = "%" + turnoLike + "%";
                    sql += " AND u.loginNombre LIKE @likeParam";
                }

                sql += @"
                    GROUP BY 
                        u.loginNombre, c.PLACAS
                    ORDER BY 
                        u.loginNombre, c.PLACAS";

                return await connection.QueryAsync(sql, new { fechaInicio, fechaFin, likeParam });
            }
        }

        public async Task<ReporteTotalesDto> ObtenerTotalesReporte(DateTime fechaConsulta, string? turnoLike)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                // Usamos la función privada para obtener el rango de fechas.
                var (fechaInicio, fechaFin) = ObtenerRangoFechas(fechaConsulta);

                var sql = @"
                    SELECT 
                        SUM(ISNULL(e.CANTIDAD, el.Cantidad)) AS TotalPacas,
                        COUNT(DISTINCT a.idCamionDia) AS TotalCamiones
                    FROM 
                        ALISTAMIENTO a
                    LEFT JOIN 
                        ALISTAMIENTO_ETIQUETA ae ON ae.idAlistamiento = a.idAlistamiento
                    LEFT JOIN 
                        etiqueta e ON e.COD_ETIQUETA = ae.etiqueta
                    LEFT JOIN 
                        ETIQUETA_LINER el ON el.COD_ETIQUETA_LINER = ae.etiqueta
                    LEFT JOIN
                        [SIE].dbo.CAMION_X_DIA cxd ON a.idCamionDia = cxd.COD_REGISTRO_CAMION
                    JOIN
                        USUARIOS u ON u.UsuarioID = a.idUsuario
                    WHERE 
                        ae.fecha >= @fechaInicio
                        AND ae.fecha < @fechaFin
                        AND a.estado LIKE '%alistado%'";

                if (!string.IsNullOrWhiteSpace(turnoLike))
                {
                    sql += " AND u.loginNombre LIKE @turnoLike";
                }

                return await connection.QueryFirstOrDefaultAsync<ReporteTotalesDto>(sql, new { fechaInicio, fechaFin, turnoLike });
            }
        }


        public async Task<IEnumerable<PlanificacionCamionDTO>> ObtenerPlanificacionCamionAsync(int codCamion)
        {
            const string sql = @"
            SELECT 
                dcxd.ITEM AS Item,
                i.f120_descripcion AS Descripcion,
                CASE 
                    WHEN i.f120_id_unidad_inventario = 'UND' 
                    THEN SUM(dcxd.CANTIDAD_PLANIFICADA) / 
                         TRY_CAST(SUBSTRING(i.f120_id_unidad_empaque, 2, LEN(i.f120_id_unidad_empaque)) AS INT)
                    ELSE NULL
                END AS PacasEsperadas,
                CASE 
                    WHEN i.f120_id_unidad_inventario = 'KLS'
                    THEN SUM(dcxd.CANTIDAD_PLANIFICADA)
                    ELSE NULL
                END AS KilosEsperados
            FROM [SIE].dbo.DETALLE_CAMION_X_DIA dcxd
            JOIN [SIE].dbo.CAMION_X_DIA cxd
                ON dcxd.COD_CAMION = cxd.COD_CAMION
            JOIN [SIE].dbo.CAMION c
                ON cxd.COD_REGISTRO_CAMION = c.COD_CAMION
            LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items i
                ON dcxd.ITEM = i.f120_id
            WHERE 
              cxd.COD_CAMION = @CodCamion
              AND cxd.ESTADO = 'C'
              AND i.f120_id_cia = 2
              AND cxd.FECHA >= DATEADD(DAY, -1, GETDATE())
            GROUP BY 
                c.placas, 
                cxd.COD_CAMION, 
                dcxd.ITEM, 
                cxd.FECHA,
                i.f120_id_unidad_empaque, 
                i.f120_descripcion,
                i.f120_id_unidad_inventario
            ORDER BY cxd.FECHA ASC;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<PlanificacionCamionDTO>(sql, new { CodCamion = codCamion });
                return result;
            }
        }


        public async Task<AlistamientoEtiqueta> InsertarAlistamientoEtiquetaAsync(int idAlistamiento, string etiqueta, DateTime fecha, string estado,
    string areaInicial, string areaFinal, int idBodegaInicial, int idBodegaFinal, int idUsuario)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var alistamientoEtiqueta = new AlistamientoEtiqueta(IdAlistamiento: idAlistamiento, Etiqueta: etiqueta, Fecha: fecha, Estado: estado,
                AreaInicial: areaInicial, AreaFinal: areaFinal, IdBodegaInicial: idBodegaInicial, IdBodegaFinal: idBodegaFinal, IdUsuario: idUsuario);

            const string sql = @"
        INSERT INTO ALISTAMIENTO_ETIQUETA
            (idAlistamiento, etiqueta, fecha, estado, areaInicial, areaFinal, idBodegaInicial, idBodegaFinal, idUsuario)
        VALUES
            (@IdAlistamiento, @Etiqueta, @Fecha, @Estado, @AreaInicial, @AreaFinal, @IdBodegaInicial, @IdBodegaFinal, @IdUsuario);";

            await connection.ExecuteAsync(sql, alistamientoEtiqueta);

            return alistamientoEtiqueta;
        }







        public async Task<IEnumerable<EtiquetaLeidaDTO>> ObtenerEtiquetasLeidas(int idAlistamiento)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = @"
                  SELECT 
                    ae.etiqueta AS ETIQUETA,
                    ISNULL(e.COD_ITEM, el.ITEM) AS ITEM,
                    ISNULL(d1.F120_DESCRIPCION, d2.F120_DESCRIPCION) AS DESCRIPCION,
                    ae.areaFinal AS AREA,
                    ae.fecha AS FECHA
                FROM ALISTAMIENTO_ETIQUETA ae
                LEFT JOIN ETIQUETA e 
                    ON ae.etiqueta = e.COD_ETIQUETA
                LEFT JOIN ETIQUETA_LINER el 
                    ON ae.etiqueta = el.COD_ETIQUETA_LINER
                LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items d1 
                    ON e.COD_ITEM = d1.F120_ID 
                    AND d1.f120_id_cia = 2
                LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items d2 
                    ON el.ITEM = d2.F120_ID 
                    AND d2.f120_id_cia = 2
                WHERE ae.idAlistamiento = @idAlistamiento
                ORDER BY ae.fecha DESC

            ";

                return await connection.QueryAsync<EtiquetaLeidaDTO>(sql, new { idAlistamiento });
            }


        }





    }

}
