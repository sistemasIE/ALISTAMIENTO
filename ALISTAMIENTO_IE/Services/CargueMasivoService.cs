using ALISTAMIENTO_IE.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALISTAMIENTO_IE.Services
{
    public class DoctoContableDto
    {
        public string f350_id_tipo_docto { get; set; } = "";
        public int f350_consec_docto { get; set; }
        public string Documento { get; set; } = ""; // CASE + concatenación
    }
    public class TerceroDto
    {
        public string f200_id { get; set; } = "";
        public string f200_razon_social { get; set; } = "";
    }
    public class ConductorDto
    {
        public long COD_CONDUCTOR { get; set; }
        public string? NOMBRES { get; set; }
        public string? TELEFONO { get; set; }
        public string? CI { get; set; }
    }
    public class CamionDto
    {
        public long COD_CAMION { get; set; }
        public string? PLACAS { get; set; }
        public byte? TIPOLOGIA { get; set; }
    }
    public class CamionPorDiaDto
    {
        public long COD_CAMION { get; set; }
        public DateTime? FECHA { get; set; }
        public string? COD_EMPRESA_TRANSPORTE { get; set; }
        public string? ESTADO { get; set; }
        public long? COD_REGISTRO_CAMION { get; set; }
        public long? COD_CONDUCTOR { get; set; }
    }
    public class MovimientoDoctoDto
    {
        public DateTime FECHA { get; set; }
        public string NUM_DOCUMENTO { get; set; } = "";
        public string ESTADO { get; set; } = "";
        public string NOMBRE_CONDUCTOR { get; set; } = ""; // queda vacío por ahora
        public string BOD_SALIDA { get; set; } = "";
        public string BOD_ENTRADA { get; set; } = "";
        public string ITEM_RESUMEN { get; set; } = "";
        public decimal CANT_SALDO { get; set; }
        public string NOTAS_DEL_DOCTO { get; set; } = "";
    }
    public class MovimientoDocumentoDto
    {
        public DateTime FECHA { get; set; }
        public string NUM_DOCUMENTO { get; set; } = "";
        public string ESTADO { get; set; } = "";
        public string NOMBRE_CONDUCTOR { get; set; } = "";
        public string BOD_SALIDA { get; set; } = "";
        public string BOD_ENTRADA { get; set; } = "";
        public string ITEM_RESUMEN { get; set; } = "";
        public decimal CANT_SALDO { get; set; }
        public string NOTAS_DEL_DOCTO { get; set; } = "";
    }
    internal class CargueMasivoService
    {
        private readonly string _connectionString;
        private readonly string _connectionStringSIIE;
        public CargueMasivoService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionUnoe"].ConnectionString;
            _connectionStringSIIE = ConfigurationManager.ConnectionStrings["stringConexionSIE"].ConnectionString;
        }
        public async Task<long> CalcularMaxCodigoAsync(string nombreTabla, string pk)
        {
            // ⚠️ Ajusta tu cadena de conexión fija aquí
            var connectionString = _connectionStringSIIE;

            const string sqlTemplate = "SELECT MAX({0}) FROM {1};";
            var sql = string.Format(sqlTemplate, pk, nombreTabla);

            await using var conexion = new SqlConnection(connectionString);
            await conexion.OpenAsync();

            await using var comando = new SqlCommand(sql, conexion);
            var result = await comando.ExecuteScalarAsync();

            long max;
            if (result == DBNull.Value || result == null)
                max = 1; // si no hay registros
            else
                max = Convert.ToInt64(result) + 1;

            return max;
        }
        /// <summary>
        /// Obtiene un documento contable específico según compañía, tipo de documento y consecutivo.
        /// </summary>
        /// <param name="idCia">ID de la compañía (1 o 2).</param>
        /// <param name="idTipoDocto">ID del tipo de documento (ej: "TTS").</param>
        /// <param name="consecDocto">Consecutivo del documento.</param>
        /// <returns>Un documento contable o null si no existe.</returns>
        public async Task<DoctoContableDto?> ObtenerDocumentoContableAsync(string idCia, string idTipoDocto, int consecDocto)
        {
            const string sql = @"
            SELECT 
                f350_id_tipo_docto,
                f350_consec_docto,
                CASE f350_id_cia 
                    WHEN '1' THEN 'RD/' 
                    WHEN '2' THEN 'IE/' 
                END + f350_id_tipo_docto + '-' + CONVERT(nvarchar, f350_consec_docto) AS Documento
            FROM t350_co_docto_contable
            WHERE 
                f350_ind_estado = 1
                AND f350_id_tipo_docto = @idTipoDocto
                AND f350_id_cia = @idCia
                AND f350_consec_docto = @consecDocto;";

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<DoctoContableDto>(sql, new { idCia, idTipoDocto, consecDocto });
        }
        public async Task<TerceroDto?> ObtenerTerceroPorRowIdAsync(int rowId)
        {
            const string sql = @"
        SELECT 
            f200_id,
            f200_razon_social
        FROM t200_mm_terceros
        WHERE f200_rowid = @rowId;";

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<TerceroDto>(sql, new { rowId });
        }
        public async Task<ConductorDto?> ObtenerConductorPorCodigoAsync(long codConductor)
        {
            const string sql = @"
        SELECT 
            COD_CONDUCTOR,
            NOMBRES,
            TELEFONO,
            CI
        FROM CONDUCTOR
        WHERE COD_CONDUCTOR = @codConductor;";

            await using var connection = new SqlConnection(_connectionStringSIIE);
            return await connection.QueryFirstOrDefaultAsync<ConductorDto>(sql, new { codConductor });
        }
        public async Task<CamionDto?> ObtenerCamionPorCodigoAsync(long codCamion)
        {
            const string sql = @"
        SELECT 
            COD_CAMION,
            PLACAS,
            TIPOLOGIA
        FROM CAMION
        WHERE COD_CAMION = @codCamion;";

            await using var connection = new SqlConnection(_connectionStringSIIE);
            return await connection.QueryFirstOrDefaultAsync<CamionDto>(sql, new { codCamion });
        }
        public async Task<int> InsertarCamionPorDiaAsync(CamionPorDiaDto camionDia)
        {
            const string sql = @"
        INSERT INTO CAMION_X_DIA
            (COD_CAMION, FECHA, COD_EMPRESA_TRANSPORTE, ESTADO, COD_REGISTRO_CAMION, COD_CONDUCTOR)
        VALUES
            (@COD_CAMION, @FECHA, @COD_EMPRESA_TRANSPORTE, @ESTADO, @COD_REGISTRO_CAMION, @COD_CONDUCTOR);";

            await using var connection = new SqlConnection(_connectionString);
            return await connection.ExecuteAsync(sql, camionDia);
        }
        public async Task<IEnumerable<MovimientoDoctoDto>> ObtenerMovimientosPorConsecutivoAsync(
    int consecDocto,
    string idTipoDocto = "TTS")
        {
            const string sql = @"
        SELECT 
    t350.f350_rowid,
    t350.f350_fecha AS FECHA,
    CASE t350.f350_id_cia WHEN 1 THEN 'RD/' WHEN 2 THEN 'IE/' END
        + t350.f350_id_tipo_docto + '-' + CONVERT(nvarchar(20), t350.f350_consec_docto) AS [NUM DOCUMENTO],
    CASE t350.f350_ind_estado WHEN 0 THEN 'EN ELABORACION' WHEN 1 THEN 'APROBADO' WHEN 2 THEN 'ANULADO' END AS [ESTADO],
    '' AS [NOMBRE_CONDUCTOR],

    -- Bodega salida (desde t470)
    bod_s.f150_descripcion AS [BOD_SALIDA],

    -- Bodega entrada (desde t450 -> t150)
    (bod_e.f150_id + '-->'+ bod_e.f150_id) AS [BOD_ENTRADA],

    CONVERT(nvarchar(50), t120.f120_id) + ' ->' + CONVERT(nvarchar(200), t120.f120_descripcion) AS [ITEM_RESUMEN],
    t470.f470_cant_base AS [CANT_SALDO],
    t350.f350_notas AS [NOTAS_DEL_DOCTO]
FROM t350_co_docto_contable AS t350
JOIN t470_cm_movto_invent      AS t470  ON t470.f470_rowid_docto     = t350.f350_rowid
JOIN t121_mc_items_extensiones AS t121  ON t121.f121_rowid           = t470.f470_rowid_item_ext
JOIN t120_mc_items             AS t120  ON t120.f120_rowid           = t121.f121_rowid_item
JOIN t450_cm_docto_invent      AS t450  ON t450.f450_rowid_docto     = t350.f350_rowid
-- t150 para bodega de salida
JOIN UnoEE_Doron.dbo.t150_mc_bodegas           AS bod_s ON bod_s.f150_rowid          = t470.f470_rowid_bodega
-- t150 para bodega de entrada
LEFT JOIN UnoEE_Doron.dbo.t150_mc_bodegas      AS bod_e ON bod_e.f150_rowid          = t450.f450_rowid_bodega_entrada
WHERE 
    t350.f350_id_tipo_docto = @idTipoDocto
    AND t350.f350_consec_docto = @consecDocto;";

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<MovimientoDoctoDto>(sql, new { consecDocto, idTipoDocto });
        }

       

        public async Task<int> GuardarCamionDiaYDetallesAsync(
    IEnumerable<GrupoMovimientosDto> grupos,
    string estadoCabecera = "C",           // estado para CAMION_X_DIA
    string estadoDetalle = "C",            // estado para DETALLE_CAMION_X_DIA
    string? unidadMedidaDefault = null
)
        {
            // Insert cabecera (siempre nueva)
            const string sqlInsertCabecera = @"
        INSERT INTO CAMION_X_DIA
            (COD_CAMION, FECHA, COD_EMPRESA_TRANSPORTE, ESTADO, COD_REGISTRO_CAMION, COD_CONDUCTOR)
        VALUES
            (@COD_CAMION, @FECHA, @COD_EMPRESA_TRANSPORTE, @ESTADO, @COD_REGISTRO_CAMION, @COD_CONDUCTOR);";

            // Insert detalle
            const string sqlInsertDetalle = @"
        INSERT INTO DETALLE_CAMION_X_DIA
            (COD_DETALLE_CAMION, COD_CAMION, ITEM, CANTIDAD_PLANIFICADA, CANTIDAD_DESPACHADA,
             JUSTIFICACION, ESTADO, ITEM_EQUIVALENTE, PTO_ENVIO, SECUENCIAL, UN_MEDIDA)
        VALUES
            (@COD_DETALLE_CAMION, @COD_CAMION, @ITEM, @CANTIDAD_PLANIFICADA, @CANTIDAD_DESPACHADA,
             @JUSTIFICACION, @ESTADO, @ITEM_EQUIVALENTE, @PTO_ENVIO, @SECUENCIAL, @UN_MEDIDA);";

            // Para los PK autogenerados (MAX+1)
            const string sqlMaxCabecera = @"SELECT ISNULL(MAX(COD_CAMION), 0) FROM CAMION_X_DIA WITH (UPDLOCK, HOLDLOCK);";
            const string sqlMaxDetalle = @"SELECT ISNULL(MAX(COD_DETALLE_CAMION), 0) FROM DETALLE_CAMION_X_DIA WITH (UPDLOCK, HOLDLOCK);";

            int filasAfectadas = 0;

            await using var cn = new SqlConnection(_connectionStringSIIE);
            await cn.OpenAsync();
            using var tx = cn.BeginTransaction();

            try
            {
                var maxCodCamion = await cn.ExecuteScalarAsync<long>(sqlMaxCabecera, transaction: tx);
                var maxCodDetalle = await cn.ExecuteScalarAsync<long>(sqlMaxDetalle, transaction: tx);

                long nextCodCamion = maxCodCamion + 1;
                long nextDetId = maxCodDetalle + 1;

                foreach (var g in grupos)
                {
                    // 1) Insertar CABECERA
                    long codCamionDia = nextCodCamion++;

                    filasAfectadas += await cn.ExecuteAsync(
                        sqlInsertCabecera,
                        new
                        {
                            COD_CAMION = codCamionDia,                 // PK generado
                            FECHA = g.Fecha,
                            COD_EMPRESA_TRANSPORTE = g.EmpresaTransporte,
                            ESTADO = estadoCabecera,
                            COD_REGISTRO_CAMION = g.CodCamion,        // el código de camión original
                            COD_CONDUCTOR = g.CodConductor
                        },
                        tx);

                    // 2) Insertar DETALLES
                    var detallesParams = new List<object>(g.Movimientos.Count);

                    foreach (var m in g.Movimientos)
                    {
                        double? cantidadPlanificada = (double?)Convert.ToDecimal(m.CANT_SALDO);
                        string item = ExtraerItemDesdeResumen(m.ITEM_RESUMEN);
                        string ptoEnvio = m.BOD_ENTRADA ?? string.Empty;
                        string secuencial = m.NUM_DOCUMENTO;

                        detallesParams.Add(new
                        {
                            COD_DETALLE_CAMION = nextDetId++,
                            COD_CAMION = codCamionDia,                // referencia al COD_CAMION de la cabecera
                            ITEM = item,
                            CANTIDAD_PLANIFICADA = cantidadPlanificada,
                            CANTIDAD_DESPACHADA = (double?)null,
                            JUSTIFICACION = "N/A",
                            ESTADO = estadoDetalle,
                            ITEM_EQUIVALENTE = (string?)null,
                            PTO_ENVIO = ptoEnvio,
                            SECUENCIAL = secuencial,
                            UN_MEDIDA = unidadMedidaDefault
                        });
                    }

                    if (detallesParams.Count > 0)
                    {
                        filasAfectadas += await cn.ExecuteAsync(sqlInsertDetalle, detallesParams, tx);
                    }
                }

                tx.Commit();
                return filasAfectadas;
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        // Helper para extraer el ITEM desde ITEM_RESUMEN
        private static string ExtraerItemDesdeResumen(string itemResumen)
        {
            if (string.IsNullOrWhiteSpace(itemResumen)) return string.Empty;
            int p = itemResumen.IndexOf("->", StringComparison.Ordinal);
            return (p > 0) ? itemResumen[..p].Trim() : itemResumen.Trim();
        }

    }


}
