using Dapper;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace ALISTAMIENTO_IE.Services
{
    public class ItemService
    {
        private readonly string _connectionStringUnoe;

        public ItemService()
        {
            _connectionStringUnoe = ConfigurationManager.ConnectionStrings["stringConexionUnoe"].ConnectionString;
        }

        public Dictionary<int, string> ObtenerDescripcionesItems(IEnumerable<int> itemIds, int idCia)
        {
            using (SqlConnection connection = new SqlConnection(_connectionStringUnoe))
            {
                string sql = @"SELECT f120_id, f120_descripcion FROM t120_mc_items WHERE f120_id_cia = @idCia AND f120_id IN @itemIds";
                var items = connection.Query<(int f120_id, string f120_descripcion)>(sql, new { idCia, itemIds });
                return items.ToDictionary(x => x.f120_id, x => x.f120_descripcion);
            }
        }


        public string[] ParseItemsFromTextArea(string rawItems)
        {
            var allItems = rawItems
                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .ToArray();

            var validItems = allItems.Where(i => int.TryParse(i, out _)).ToArray();
            var invalidItems = allItems.Except(validItems).ToArray();

            if (invalidItems.Length > 0)
            {
                MessageBox.Show($"Los siguientes items no son válidos y serán ignorados:\n\n{string.Join("\r\n", invalidItems)}",
                    "Items inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return validItems;
        }
    }
}
