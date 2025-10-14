using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace ALISTAMIENTO_IE.Services
{
    public interface IKardexService
    {
        List<int> ObtenerItemsValidos(string entrada, out List<string> itemsInvalidos);
        DataTable ObtenerDatosDeItems(IEnumerable<string> items);
        Task<int> UpdateAsync(KardexBodega kardex);
        Task<KardexBodega?> ObtenerKardexDeEtiqueta(string etiqueta);
    }

    public class KardexService : IKardexService
    {
        private readonly string _connectionString;

        public KardexService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
        }

        public List<int> ObtenerItemsValidos(string entrada, out List<string> itemsInvalidos)
        {
            var todos = entrada
                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .ToList();

            var validos = todos.Where(i => int.TryParse(i, out _)).Select(int.Parse).ToList();
            itemsInvalidos = todos.Except(validos.Select(x => x.ToString())).ToList();

            return validos;
        }

        public async Task<int> UpdateAsync(KardexBodega kardex)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"
                UPDATE KARDEX_BODEGA
                SET tipoEntrada = @TipoEntrada,
                    tipoSalida = @TipoSalida,
                    etiqueta = @Etiqueta,
                    idBodega = @IdBodega,
                    fechaIngreso = @FechaIngreso,
                    fechaSalida = @FechaSalida,
                    enBodega = @EnBodega,
                    idUsuarioEntrante = @IdUsuarioEntrante,
                    idUsuarioSalida = @IdUsuarioSalida,
                    idRemision = @IdRemision,
                    area = @Area
                WHERE idKardexBodega = @IdKardexBodega";

            return await connection.ExecuteAsync(sql, kardex);
        }

        public async Task<KardexBodega?> ObtenerKardexDeEtiqueta(string etiqueta)
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"SELECT * 
                                 FROM KARDEX_BODEGA 
                                 WHERE etiqueta = @etiqueta";

            return await connection.QueryFirstOrDefaultAsync<KardexBodega>(sql, new { etiqueta });
        }

        public DataTable ObtenerDatosDeItems(IEnumerable<string> items)
        {
            var parametros = new List<SqlParameter>();
            string inClauseSacos = string.Join(",", items.Select((i, idx) => "@itemS" + idx));
            string inClauseLiner = string.Join(",", items.Select((i, idx) => "@itemL" + idx));

            int index = 0;
            foreach (var item in items)
            {
                parametros.Add(new SqlParameter("@itemS" + index, item));
                parametros.Add(new SqlParameter("@itemL" + index, item));
                index++;
            }

            string consulta = $@"
                (SELECT e.COD_ITEM AS ITEM, COUNT(e.COD_ETIQUETA) AS PACAS,
                        r.f120_descripcion AS DESCRIPCION, ub.DESCRIPCION AS BODEGA, kb.area AS AREA
                 FROM KARDEX_BODEGA kb
                 JOIN ETIQUETA e ON e.COD_ETIQUETA = kb.etiqueta
                 JOIN UBICACIONES_BODEGA ub ON kb.idBodega = ub.COD_UBICACION
                 LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items r ON e.COD_ITEM = r.f120_id
                 WHERE e.COD_ITEM IN ({inClauseSacos}) AND ENBODEGA = 1 AND f120_id_cia = 2
                 GROUP BY e.COD_ITEM, kb.area, r.f120_descripcion, ub.DESCRIPCION)
                UNION
                (SELECT eL.ITEM AS ITEM, COUNT(eL.COD_ETIQUETA_LINER) AS PACAS,
                        r.f120_descripcion AS DESCRIPCION, ub.DESCRIPCION AS BODEGA, kb.area AS AREA
                 FROM KARDEX_BODEGA kb
                 JOIN ETIQUETA_LINER eL ON eL.COD_ETIQUETA_LINER = kb.etiqueta
                 JOIN UBICACIONES_BODEGA ub ON kb.idBodega = ub.COD_UBICACION
                 LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items r ON eL.ITEM = r.f120_id
                 WHERE eL.ITEM IN ({inClauseLiner}) AND ENBODEGA = 1 AND f120_id_cia = 2
                 GROUP BY eL.ITEM, kb.area, r.f120_descripcion, ub.DESCRIPCION)
                ORDER BY ITEM, AREA DESC, BODEGA DESC, PACAS DESC";

            using var conexion = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(consulta, conexion);
            cmd.Parameters.AddRange(parametros.ToArray());

            using var adaptador = new SqlDataAdapter(cmd);
            var tabla = new DataTable();
            adaptador.Fill(tabla);
            return tabla;
        }


    }
}
