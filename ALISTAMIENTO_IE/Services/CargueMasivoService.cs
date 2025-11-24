using ALISTAMIENTO_IE.DTOs;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data.SqlTypes;

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
        public string TIPO_DOCUMENTO { get; set; } = "";
        public string EMPRESA_TRANSPORTE { get; set; } = "";

        public string ESTADO { get; set; } = "";
        public string NOMBRE_CONDUCTOR { get; set; } = "";
        public string BOD_SALIDA { get; set; } = "";
        public string BOD_ENTRADA { get; set; } = "";
        public string ITEM_RESUMEN { get; set; } = "";
        public decimal CANT_SALDO { get; set; }
        public string NOTAS_DEL_DOCTO { get; set; } = "";
    }

    public class DocumentoDespachadoDto
    {
        public long COD_DOCUMENTO_DESPACHADO { get; set; }
        public string SECUENCIAL { get; set; } = "";
        public long COD_CAMION { get; set; }
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
        public async Task<DocumentoDespachadoDto?> ObtenerDocumentoDespachado(string  secuencial)
        {
            const string sql = @"
                                select COD_DOCUMENTO_DESPACHADO, SECUENCIAL, COD_CAMION 
                                from DOCUMENTOS_DESPACHADOS WHERE SECUENCIAL = @secuencial;";

            await using var connection = new SqlConnection(_connectionStringSIIE);
            return await connection.QueryFirstOrDefaultAsync<DocumentoDespachadoDto>(sql, new { secuencial });
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
    string idTipoDocto,
    int idCia)
        {
            await using var connection = new SqlConnection(_connectionString);

            if (string.Equals(idTipoDocto, "RMV", StringComparison.OrdinalIgnoreCase))
            {
                // === Query RMV ===
                const string sqlRmv = @"
            select f350_fecha AS FECHA,'' as [NOMBRE CONDUCTOR],
             case f350_id_cia when '1' then 'RD/' when '2' then 'IE/' end+f350_id_tipo_docto+'-'+CONVERT(nvarchar, f350_consec_docto) AS [NUM DOCUMENTO],
             f200_nit, f200_razon_social as[Razon Social cliente despacho],'' as [U.M.emp.],f470_cant_base AS [CANT_SALDO],'' as [Peso en KLS],'' as [Cantidad en emp.],
             f150_descripcion as [BOD_SALIDA],f120_id as [ITEM],'' as [ITEM EQUIVALENTE IE],convert(nvarchar, f120_id)+'->'+convert(nvarchar, f120_descripcion) AS ITEM_RESUMEN,
             '' as [ANCHO], '' as [LARGO],'' AS [GRAMAJE],f350_notas AS [NOTAS_DEL_DOCTO],f215_descripcion ,f350_id_cia,
             t013_mm_ciudades.f013_descripcion as [BOD_ENTRADA]
             from t350_co_docto_contable, t470_cm_movto_invent,t200_mm_terceros,t120_mc_items,t121_mc_items_extensiones,t150_mc_bodegas,t215_mm_puntos_envio_cliente,
             t015_mm_contactos,t013_mm_ciudades
             where t350_co_docto_contable.f350_rowid= t470_cm_movto_invent.f470_rowid_docto  
             and t350_co_docto_contable.f350_rowid_tercero=t200_mm_terceros.f200_rowid
             and t470_cm_movto_invent.f470_rowid_item_ext= t121_mc_items_extensiones.f121_rowid 
             and t121_mc_items_extensiones.f121_rowid_item=t120_mc_items.f120_rowid
             and t470_cm_movto_invent.f470_rowid_bodega=t150_mc_bodegas.f150_rowid
             and f470_rowid_punto_envio_rem=f215_rowid
             and f470_rowid_bodega=f150_rowid and f215_rowid_tercero=t200_mm_terceros.f200_rowid
             and t215_mm_puntos_envio_cliente.f215_rowid_contacto  = t015_mm_contactos.f015_rowid 
             and t015_mm_contactos.f015_id_ciudad = t013_mm_ciudades.f013_id 
             and t015_mm_contactos.f015_id_depto  = t013_mm_ciudades.f013_id_depto 
             and  t015_mm_contactos.f015_id_pais = t013_mm_ciudades.f013_id_pais 
              and f350_id_tipo_docto =UPPER(@idTipoDocto)
              and f350_consec_docto = @consecDocto
              and f350_id_cia =@idCia";

                return await connection.QueryAsync<MovimientoDoctoDto>(sqlRmv, new { consecDocto, idTipoDocto, idCia });
            }
            else  if (string.Equals(idTipoDocto, "RMV", StringComparison.OrdinalIgnoreCase))
            {
                // === Query TTS (u otros) ===
                const string sqlTts = @"
            SELECT 
                t350.f350_rowid,
                t350.f350_fecha AS FECHA,
                CASE t350.f350_id_cia WHEN 1 THEN 'RD/' WHEN 2 THEN 'IE/' END
                    + t350.f350_id_tipo_docto + '-' + CONVERT(nvarchar(20), t350.f350_consec_docto) AS NUM_DOCUMENTO,
                CASE t350.f350_ind_estado 
                    WHEN 0 THEN 'EN ELABORACION' 
                    WHEN 1 THEN 'APROBADO' 
                    WHEN 2 THEN 'ANULADO' 
                END AS ESTADO,
                '' AS NOMBRE_CONDUCTOR,
                bod_s.f150_descripcion AS BOD_SALIDA,
                (bod_e.f150_id + '-->' + bod_e.f150_id) AS BOD_ENTRADA,
                CONVERT(nvarchar(50), t120.f120_id) + ' ->' + CONVERT(nvarchar(200), t120.f120_descripcion) AS ITEM_RESUMEN,
                t470.f470_cant_base AS CANT_SALDO,
                t350.f350_notas AS NOTAS_DEL_DOCTO
            FROM t350_co_docto_contable        AS t350
            JOIN t470_cm_movto_invent          AS t470  ON t470.f470_rowid_docto     = t350.f350_rowid
            JOIN t121_mc_items_extensiones     AS t121  ON t121.f121_rowid           = t470.f470_rowid_item_ext
            JOIN t120_mc_items                 AS t120  ON t120.f120_rowid           = t121.f121_rowid_item
            JOIN t450_cm_docto_invent          AS t450  ON t450.f450_rowid_docto     = t350.f350_rowid
            JOIN t150_mc_bodegas               AS bod_s ON bod_s.f150_rowid          = t470.f470_rowid_bodega
            LEFT JOIN t150_mc_bodegas          AS bod_e ON bod_e.f150_rowid          = t450.f450_rowid_bodega_entrada
            WHERE 
                UPPER(t350.f350_id_tipo_docto) = UPPER(@idTipoDocto)
                AND t350.f350_consec_docto     = @consecDocto
                AND t350.f350_id_cia           = @idCia
            ORDER BY t350.f350_fecha DESC, t120.f120_id;";

                return await connection.QueryAsync<MovimientoDoctoDto>(sqlTts, new { consecDocto, idTipoDocto, idCia });
            }
            else
            {
                // === Query RMV ===
                const string sqlSam = @"
            select f350_fecha AS FECHA,'' as [NOMBRE CONDUCTOR],
             case f350_id_cia when '1' then 'RD/' when '2' then 'IE/' end+f350_id_tipo_docto+'-'+CONVERT(nvarchar, f350_consec_docto) AS [NUM DOCUMENTO],
             f200_nit, f200_razon_social as[Razon Social cliente despacho],'' as [U.M.emp.],f470_cant_base AS [CANT_SALDO],'' as [Peso en KLS],'' as [Cantidad en emp.],
             f150_descripcion as [BOD_SALIDA],f120_id as [ITEM],'' as [ITEM EQUIVALENTE IE],convert(nvarchar, f120_id)+'->'+convert(nvarchar, f120_descripcion) AS ITEM_RESUMEN,
             '' as [ANCHO], '' as [LARGO],'' AS [GRAMAJE],f350_notas AS [NOTAS_DEL_DOCTO],f215_descripcion ,f350_id_cia,
             t013_mm_ciudades.f013_descripcion as [BOD_ENTRADA]
             from t350_co_docto_contable, t470_cm_movto_invent,t200_mm_terceros,t120_mc_items,t121_mc_items_extensiones,t150_mc_bodegas,t215_mm_puntos_envio_cliente,
             t015_mm_contactos,t013_mm_ciudades
             where t350_co_docto_contable.f350_rowid= t470_cm_movto_invent.f470_rowid_docto  
             and t350_co_docto_contable.f350_rowid_tercero=t200_mm_terceros.f200_rowid
             and t470_cm_movto_invent.f470_rowid_item_ext= t121_mc_items_extensiones.f121_rowid 
             and t121_mc_items_extensiones.f121_rowid_item=t120_mc_items.f120_rowid
             and t470_cm_movto_invent.f470_rowid_bodega=t150_mc_bodegas.f150_rowid
             and f470_rowid_punto_envio_rem=f215_rowid
             and f470_rowid_bodega=f150_rowid and f215_rowid_tercero=t200_mm_terceros.f200_rowid
             and t215_mm_puntos_envio_cliente.f215_rowid_contacto  = t015_mm_contactos.f015_rowid 
             and t015_mm_contactos.f015_id_ciudad = t013_mm_ciudades.f013_id 
             and t015_mm_contactos.f015_id_depto  = t013_mm_ciudades.f013_id_depto 
             and  t015_mm_contactos.f015_id_pais = t013_mm_ciudades.f013_id_pais 
              and f350_id_tipo_docto =UPPER(@idTipoDocto)
              and f350_consec_docto = @consecDocto
              and f350_id_cia =@idCia";

                return await connection.QueryAsync<MovimientoDoctoDto>(sqlSam, new { consecDocto, idTipoDocto, idCia });

            }
        }



        public async Task<int> GuardarCamionDiaYDetallesAsync(
    IEnumerable<GrupoMovimientosDto> grupos,
    string estadoCabecera = "C",    // estado para CAMION_X_DIA
    string estadoDetalle = "C",     // estado para DETALLE_CAMION_X_DIA
    string? unidadMedidaDefault = null
)
        {
            // 1) Inserts
            const string sqlInsertCabecera = @"
        INSERT INTO CAMION_X_DIA
            (COD_CAMION, FECHA, COD_EMPRESA_TRANSPORTE, ESTADO, COD_REGISTRO_CAMION, COD_CONDUCTOR)
        VALUES
            (@COD_CAMION, @FECHA, @COD_EMPRESA_TRANSPORTE, @ESTADO, @COD_REGISTRO_CAMION, @COD_CONDUCTOR);";

            const string sqlInsertDetalle = @"
        INSERT INTO DETALLE_CAMION_X_DIA
            (COD_DETALLE_CAMION, COD_CAMION, ITEM, CANTIDAD_PLANIFICADA, CANTIDAD_DESPACHADA,
             JUSTIFICACION, ESTADO, ITEM_EQUIVALENTE, PTO_ENVIO, SECUENCIAL, UN_MEDIDA)
        VALUES
            (@COD_DETALLE_CAMION, @COD_CAMION, @ITEM, @CANTIDAD_PLANIFICADA, @CANTIDAD_DESPACHADA,
             @JUSTIFICACION, @ESTADO, @ITEM_EQUIVALENTE, @PTO_ENVIO, @SECUENCIAL, @UN_MEDIDA);";

            const string sqlInsertDocDespachado = @"
        INSERT INTO DOCUMENTOS_DESPACHADOS
            (COD_DOCUMENTO_DESPACHADO, SECUENCIAL, COD_CAMION)
        VALUES
            (@COD_DOCUMENTO_DESPACHADO, @SECUENCIAL, @COD_CAMION);";

            // 2) MAX+1 (con bloqueos para reservar rango en la transacción)
            const string sqlMaxCabecera = @"SELECT ISNULL(MAX(COD_CAMION), 0) FROM CAMION_X_DIA WITH (UPDLOCK, HOLDLOCK);";
            const string sqlMaxDetalle = @"SELECT ISNULL(MAX(COD_DETALLE_CAMION), 0) FROM DETALLE_CAMION_X_DIA WITH (UPDLOCK, HOLDLOCK);";
            const string sqlMaxDocDesp = @"SELECT ISNULL(MAX(COD_DOCUMENTO_DESPACHADO), 0) FROM DOCUMENTOS_DESPACHADOS WITH (UPDLOCK, HOLDLOCK);";

            int filasAfectadas = 0;

            await using var cn = new SqlConnection(_connectionStringSIIE);
            await cn.OpenAsync();
            using var tx = cn.BeginTransaction();

            try
            {
                // Reservas de ID
                var maxCodCamion = await cn.ExecuteScalarAsync<long>(sqlMaxCabecera, transaction: tx);
                var maxCodDetalle = await cn.ExecuteScalarAsync<long>(sqlMaxDetalle, transaction: tx);
                var maxCodDocDesp = await cn.ExecuteScalarAsync<long>(sqlMaxDocDesp, transaction: tx);

                long nextCodCamion = maxCodCamion + 1;
                long nextDetId = maxCodDetalle + 1;
                long nextDocId = maxCodDocDesp + 1;

                foreach (var g in grupos)
                {
                    // === 1) CABECERA ===
                    long codCamionDia = nextCodCamion++;

                    filasAfectadas += await cn.ExecuteAsync(
                        sqlInsertCabecera,
                        new
                        {
                            COD_CAMION = codCamionDia,                 // PK generado para X_DIA
                            FECHA = g.Fecha,
                            COD_EMPRESA_TRANSPORTE = g.EmpresaTransporte, // ideal: f200_id
                            ESTADO = estadoCabecera,
                            COD_REGISTRO_CAMION = g.CodCamion,         // código real de camión
                            COD_CONDUCTOR = g.CodConductor
                        },
                        tx);

                    // === 2) DETALLES ===
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
                            COD_CAMION = codCamionDia,                // FK al X_DIA recién creado
                            ITEM = item,
                            CANTIDAD_PLANIFICADA = cantidadPlanificada, // **planificada**
                            CANTIDAD_DESPACHADA = (double?)null,
                            JUSTIFICACION = "N/A",                      // **N/A**
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

                    // === 3) DOCUMENTOS_DESPACHADOS ===
                    // Para evitar duplicar el mismo documento varias veces (si hay múltiples líneas),
                    // tomamos los SECUENCIALES distintos del grupo:
                    var secuencialesUnicos = g.Movimientos
                        .Select(m => m.NUM_DOCUMENTO)
                        .Where(s => !string.IsNullOrWhiteSpace(s))
                        .Distinct()
                        .ToList();

                    var docsParams = new List<object>(secuencialesUnicos.Count);

                    foreach (var sec in secuencialesUnicos)
                    {
                        docsParams.Add(new
                        {
                            COD_DOCUMENTO_DESPACHADO = nextDocId++,
                            SECUENCIAL = sec,
                            COD_CAMION = codCamionDia
                        });
                    }

                    if (docsParams.Count > 0)
                    {
                        filasAfectadas += await cn.ExecuteAsync(sqlInsertDocDespachado, docsParams, tx);
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

        public async Task<string> SacaItemEquivalenteAsync(string itemDocumento)
        {
            if (string.IsNullOrWhiteSpace(itemDocumento))
                return "N/A";

            // Quitar último carácter del item (igual que en tu código original)


            const string sql = @"
        SELECT TOP 1 t106.f106_descripcion
        FROM t105_mc_criterios_item_planes AS t105
        INNER JOIN t106_mc_criterios_item_mayores AS t106
            ON t105.f105_id_cia = t106.f106_id_cia
           AND t105.f105_id     = t106.f106_id_plan
        INNER JOIN t125_mc_items_criterios AS t125
            ON t106.f106_id_cia   = t125.f125_id_cia
           AND t106.f106_id_plan  = t125.f125_id_plan
           AND t106.f106_id       = t125.f125_id_criterio_mayor
        INNER JOIN t120_mc_items AS t120
            ON t125.f125_rowid_item = t120.f120_rowid
        WHERE t120.f120_id = @Item
          AND t105.f105_descripcion = 'ITEM IE';";  // valor fijo

            try
            {
                await using var cn = new SqlConnection(_connectionString);
                var desc = await cn.QueryFirstOrDefaultAsync<string>(sql, new { Item = itemDocumento });
                return string.IsNullOrWhiteSpace(desc) ? "N/A" : desc;
            }
            catch
            {
                return "N/A";
            }
        }

        // Helper para extraer el ITEM desde ITEM_RESUMEN
        public static string ExtraerItemDesdeResumen(string itemResumen)
        {
            if (string.IsNullOrWhiteSpace(itemResumen)) return string.Empty;
            int p = itemResumen.IndexOf("->", StringComparison.Ordinal);
            return (p > 0) ? itemResumen[..p].Trim() : itemResumen.Trim();
        }

        public async Task<string?> ObtenerDescripcionItemAsync(string itemId)
        {

            const string sql = @"
        SELECT TOP 1 f120_descripcion
        FROM t120_mc_items
        WHERE f120_id = @ItemId
        and f120_id_cia = 2
            ;";

            try
            {
                await using var connection = new SqlConnection(_connectionString);
                return await connection.QueryFirstOrDefaultAsync<string>(sql, new { ItemId = itemId });
            }
            catch
            {
                return null;
            }
        }
        public string[] ejecuta_script(string sqlsentence)
        {
            using (SqlConnection conexion = new SqlConnection(_connectionStringSIIE))
            {
                conexion.Open();
                using (SqlCommand comando = new SqlCommand(sqlsentence, conexion))
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        int columnas = lector.FieldCount;
                        string[] valores = new string[columnas];

                        for (int i = 0; i < columnas; i++)
                        {
                            valores[i] = lector.IsDBNull(i) ? "0" : lector.GetValue(i).ToString().Trim();
                        }

                        return valores; // Retorna arreglo con los valores de esa única fila
                    }
                }
            }

            // Si no hay registros, devuelve un arreglo vacío
            return Array.Empty<string>();
        }
        public async Task<bool> AnularCamionConDocumentosAsync(long codCamion)
        {
            using (var connection = new SqlConnection(_connectionStringSIIE))
            {
                await connection.OpenAsync();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // 1. Actualiza el estado del camión a 'X'
                        string sqlUpdate = @"
                    UPDATE CAMION_X_DIA 
                    SET ESTADO = 'X' 
                    WHERE COD_CAMION = @CodCamion";

                        await connection.ExecuteAsync(sqlUpdate, new { CodCamion = codCamion }, transaction);

                        // 2. Elimina documentos despachados asociados
                        string sqlDelete = @"
                    DELETE FROM DOCUMENTOS_DESPACHADOS 
                    WHERE COD_CAMION = @CodCamion";

                        await connection.ExecuteAsync(sqlDelete, new { CodCamion = codCamion }, transaction);

                        // 3. Confirma la transacción
                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        // Si algo falla, revierte los cambios
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }
    }


}
