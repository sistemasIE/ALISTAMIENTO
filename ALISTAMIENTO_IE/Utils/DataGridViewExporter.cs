using ALISTAMIENTO_IE.Interfaces;
using ClosedXML.Excel;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;

public class DataGridViewExporter : IDataGridViewExporter
{
    public void ExportToCsv(DataGridView dgv, string filePath)
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

    public void ExportToExcel(DataGridView dgv, string filePath)
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

        // 👉 Abrir automáticamente el archivo Excel
        System.Diagnostics.Process.Start(new ProcessStartInfo()
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }

    public DataTable ConvertDynamicToDataTable(IEnumerable<dynamic> items)
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


    public DataTable ToDataTable<T>(IEnumerable<T> list)
    {
        var tb = new DataTable(typeof(T).Name);
        PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var prop in props)
        {
            tb.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
        }

        foreach (var item in list)
        {
            var values = new object[props.Length];
            for (var i = 0; i < props.Length; i++)
            {
                values[i] = props[i].GetValue(item, null) ?? DBNull.Value;
            }
            tb.Rows.Add(values);
        }

        return tb;
    }

    public DataTable ConvertToDataTable(DataGridView dgv)
    {
        DataTable dt = new DataTable();

        foreach (DataGridViewColumn col in dgv.Columns)
            dt.Columns.Add(col.HeaderText);

        foreach (DataGridViewRow row in dgv.Rows)
        {
            if (!row.IsNewRow)
            {
                var valores = row.Cells.Cast<DataGridViewCell>()
                                       .Select(c => c.Value?.ToString() ?? "")
                                       .ToArray();
                dt.Rows.Add(valores);
            }
        }

        return dt;
    }
    public void ExportarExcelConDialog(DataGridView dgv)
    {
        using (SaveFileDialog sfd = new SaveFileDialog())
        {
            sfd.Filter = "Excel (*.xlsx)|*.xlsx";
            sfd.Title = "Guardar reporte en Excel";
            sfd.FileName = "Reporte.xlsx";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Crear DataTable desde el DataGridView
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn col in dgv.Columns)
                    dt.Columns.Add(col.HeaderText);

                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        DataRow dr = dt.NewRow();
                        for (int i = 0; i < dgv.Columns.Count; i++)
                            dr[i] = row.Cells[i].Value ?? "";
                        dt.Rows.Add(dr);
                    }
                }

                // Exportar usando ClosedXML
                using (var wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add(dt, "Hoja1");
                    ws.Columns().AdjustToContents();
                    wb.SaveAs(sfd.FileName);
                }

                // Abrir el archivo Excel
                System.Diagnostics.Process.Start(new ProcessStartInfo()
                {
                    FileName = sfd.FileName,
                    UseShellExecute = true
                });
            }
        }
    }

}
