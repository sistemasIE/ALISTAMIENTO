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
            t350.f350_fecha AS FECHA,
            CASE t350.f350_id_cia 
                WHEN '1' THEN 'RD/' 
                WHEN '2' THEN 'IE/' 
            END + t350.f350_id_tipo_docto + '-' + CONVERT(nvarchar, t350.f350_consec_docto) AS NUM_DOCUMENTO,
            CASE t350.f350_ind_estado 
                WHEN 0 THEN 'EN ELABORACION' 
                WHEN 1 THEN 'APROBADO' 
                WHEN 2 THEN 'ANULADO' 
            END AS ESTADO,
            '' AS NOMBRE_CONDUCTOR,
            t150.f150_descripcion AS BOD_SALIDA,
            t470.f470_id_ubicacion_aux AS BOD_ENTRADA,
            CONVERT(nvarchar, t120.f120_id) + '->' + CONVERT(nvarchar, t120.f120_descripcion) AS ITEM_RESUMEN,
            t470.f470_cant_base AS CANT_SALDO,
            t350.f350_notas AS NOTAS_DEL_DOCTO
        FROM t350_co_docto_contable        AS t350
        JOIN t470_cm_movto_invent          AS t470 ON t350.f350_rowid         = t470.f470_rowid_docto
        JOIN t121_mc_items_extensiones     AS t121 ON t470.f470_rowid_item_ext = t121.f121_rowid
        JOIN t120_mc_items                 AS t120 ON t121.f121_rowid_item    = t120.f120_rowid
        JOIN t150_mc_bodegas               AS t150 ON t470.f470_rowid_bodega  = t150.f150_rowid
        WHERE UPPER(t350.f350_id_tipo_docto) = UPPER(@idTipoDocto)
          AND t350.f350_consec_docto = @consecDocto
        ORDER BY t350.f350_fecha DESC, t120.f120_id;";

            await using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<MovimientoDoctoDto>(sql, new { consecDocto, idTipoDocto });
        }
    }

}
