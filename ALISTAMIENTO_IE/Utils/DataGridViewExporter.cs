using ClosedXML.Excel;
using System.Data;
using System.Text;

public static class DataGridViewExporter
{
    public static void ExportToCsv(DataGridView dgv, string filePath)
    {
        var sb = new StringBuilder();
        // Header
        for (int i = 0; i < dgv.Columns.Count; i++)
        {
            sb.Append(dgv.Columns[i].HeaderText);
            if (i < dgv.Columns.Count - 1)
                sb.Append(",");
        }
        sb.AppendLine();
        // Rows
        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (!row.IsNewRow)
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                {
                    sb.Append(row.Cells[i].Value?.ToString().Replace(",", " ") ?? "");
                    if (i < dgv.Columns.Count - 1)
                        sb.Append(",");
                }
                sb.AppendLine();
            }
        }
        File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
    }

    public static void ExportToExcel(DataGridView dgv, string filePath)
    {
        var dt = new DataTable();
        // Add columns
        foreach (DataGridViewColumn col in dgv.Columns)
            dt.Columns.Add(col.HeaderText);
        // Add rows
        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (!row.IsNewRow)
            {
                var dr = dt.NewRow();
                for (int i = 0; i < dgv.Columns.Count; i++)
                    dr[i] = row.Cells[i].Value ?? "";
                dt.Rows.Add(dr);
            }
        }
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add(dt, "Items");
            ws.Columns().AdjustToContents();
            wb.SaveAs(filePath);
        }
    }



    public static DataTable ConvertDynamicToDataTable(IEnumerable<dynamic> items)
    {
        var dt = new DataTable();
        bool colsCreated = false;
        foreach (var item in items)
        {
            if (item is IDictionary<string, object> dict)
            {
                if (!colsCreated)
                {
                    foreach (var key in dict.Keys)
                    {
                        dt.Columns.Add(key);
                    }
                    colsCreated = true;
                }
                var row = dt.NewRow();
                foreach (var kv in dict)
                {
                    row[kv.Key] = kv.Value ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
            else
            {
                // Fallback via reflection
                var props = item.GetType().GetProperties();
                if (!colsCreated)
                {
                    foreach (var p in props)
                    {
                        dt.Columns.Add(p.Name);
                    }
                    colsCreated = true;
                }
                var row = dt.NewRow();
                foreach (var p in props)
                {
                    row[p.Name] = p.GetValue(item) ?? DBNull.Value;
                }
                dt.Rows.Add(row);
            }
        }
        return dt;
    }
}
