using System.Data;

namespace ALISTAMIENTO_IE.Utils
{
    internal class DataTableExporter
    {
        public static DataTable FilterDataTableColumns(DataTable originalTable, IEnumerable<string> columnsToExclude)
        {
            DataTable filteredTable = originalTable.Clone();
            HashSet<string> excludedColumns = new HashSet<string>(columnsToExclude, StringComparer.OrdinalIgnoreCase);

            List<DataColumn> columnsToRemove = new List<DataColumn>();
            foreach (DataColumn column in filteredTable.Columns)
            {
                if (excludedColumns.Contains(column.ColumnName))
                {
                    columnsToRemove.Add(column);
                }
            }

            foreach (DataColumn column in columnsToRemove)
            {
                filteredTable.Columns.Remove(column.ColumnName);
            }

            foreach (DataRow row in originalTable.Rows)
            {
                filteredTable.ImportRow(row);
            }

            return filteredTable;
        }

        /// <summary>
        /// Agrega una columna genérica al DataTable asegurando que tenga la misma cantidad de filas.
        /// </summary>
        /// <typeparam name="T">Tipo de los datos de la nueva columna.</typeparam>
        /// <param name="table">DataTable al que se agregará la columna.</param>
        /// <param name="columnName">Nombre de la nueva columna.</param>
        /// <param name="values">Valores que se asignarán a la columna. Deben coincidir en cantidad con las filas del DataTable.</param>
        /// <returns>El DataTable con la nueva columna agregada.</returns>
        public static DataTable AddColumnToDataTable<T>(DataTable table, string columnName, IEnumerable<T> values)
        {
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            if (string.IsNullOrWhiteSpace(columnName))
                throw new ArgumentException("El nombre de la columna no puede estar vacío.", nameof(columnName));

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            var valueList = new List<T>(values);
            if (valueList.Count != table.Rows.Count)
                throw new ArgumentException("La cantidad de valores no coincide con la cantidad de filas del DataTable.");

            // Crear la nueva columna
            DataColumn newColumn = new DataColumn(columnName, typeof(T));
            table.Columns.Add(newColumn);

            // Asignar los valores a cada fila
            for (int i = 0; i < table.Rows.Count; i++)
            {
                table.Rows[i][columnName] = valueList[i];
            }

            return table;
        }
    }
}