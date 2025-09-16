using Microsoft.Data.SqlClient;
using System.Data;

namespace LECTURA_DE_BANDA
{
    public partial class CONSULTA_ITEMS_ETIQUETAS : Form
    {
        private readonly string? _placaCamion;

        public CONSULTA_ITEMS_ETIQUETAS()
        {
            InitializeComponent();

            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;

            btnBuscarEtiquetado.Click += btnBuscarItems_Click;
            btnImprimirEtiquetado.Click += btnImprimirEtiquetado_Click;
        }

        public CONSULTA_ITEMS_ETIQUETAS(IEnumerable<string> items) : this()
        {
            if (items != null)
            {
                txtItems.Text = string.Join("\r\n", items);
                btnBuscarItems_Click(this, EventArgs.Empty);
            }
        }

        public CONSULTA_ITEMS_ETIQUETAS(IEnumerable<string> items, string placaCamion) : this()
        {
            _placaCamion = placaCamion;
            if (items != null)
            {
                txtItems.Text = string.Join("\r\n", items);
                btnBuscarItems_Click(this, EventArgs.Empty);
            }
        }


        private void btnBuscarItems_Click(object sender, EventArgs e)
        {
            var allItems = txtItems.Text
                .Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(i => i.Trim())
                .Where(i => !string.IsNullOrWhiteSpace(i))
                .ToArray();

            var validItems = allItems.Where(i => int.TryParse(i, out _)).ToArray();
            var invalidItems = allItems.Except(validItems).ToArray();

            if (invalidItems.Length > 0)
            {
                MessageBox.Show($"Los siguientes items no son válidos y serán ignorados:\n\n{string.Join("\r\n", invalidItems)}", "Items inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtItems.Text = string.Join("\r\n", validItems);
            }

            if (validItems.Length == 0)
            {
                MessageBox.Show("Ingrese al menos un ITEM válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["stringConexionLocal"].ConnectionString;
            var parameters = new List<SqlParameter>();
            string inClauseSacos = string.Join(",", validItems.Select((i, idx) => "@itemS" + idx));
            string inClauseLiner = string.Join(",", validItems.Select((i, idx) => "@itemL" + idx));
            for (int i = 0; i < validItems.Length; i++)
            {
                parameters.Add(new SqlParameter("@itemS" + i, validItems[i]));
                parameters.Add(new SqlParameter("@itemL" + i, validItems[i]));
            }

            string query = $@"
SELECT * FROM
(
    (
        SELECT
            e.COD_ITEM AS ITEM,
            COUNT(e.COD_ETIQUETA) AS pacas,
            r.f120_descripcion AS DESCRIPCION,
            ub.DESCRIPCION AS BODEGA,
            kb.area AS AREA
        FROM KARDEX_BODEGA kb
        JOIN ETIQUETA e ON e.COD_ETIQUETA = kb.etiqueta
        JOIN UBICACIONES_BODEGA ub ON kb.idBodega = ub.COD_UBICACION
        LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items r ON e.COD_ITEM = r.f120_id
        WHERE
            e.COD_ITEM IN ({inClauseSacos})
            AND ENBODEGA = 1
            AND f120_id_cia = 2
        GROUP BY
            e.COD_ITEM,
            kb.area,
            r.f120_descripcion,
            ub.DESCRIPCION,
            MONTH(kb.fechaIngreso),
            DAY(kb.fechaIngreso)
    )
    UNION ALL
    (
        SELECT
            eL.ITEM AS ITEM,
            COUNT(eL.COD_ETIQUETA_LINER) AS pacas,
            r.f120_descripcion AS DESCRIPCION,
            ub.DESCRIPCION AS BODEGA,
            kb.area AS AREA
        FROM KARDEX_BODEGA kb
        JOIN ETIQUETA_LINER eL ON eL.COD_ETIQUETA_LINER = kb.etiqueta
        JOIN UBICACIONES_BODEGA ub ON kb.idBodega = ub.COD_UBICACION
        LEFT JOIN [192.168.50.86].REPLICA.dbo.t120_mc_items r ON eL.ITEM = r.f120_id
        WHERE
            eL.ITEM IN ({inClauseLiner})
            AND ENBODEGA = 1
            AND f120_id_cia = 2
        GROUP BY
            eL.ITEM,
            kb.area,
            r.f120_descripcion,
            ub.DESCRIPCION,
            MONTH(kb.fechaIngreso),
            DAY(kb.fechaIngreso)
    )
) AS Subconsulta
ORDER BY
    ITEM,
    AREA DESC,
    BODEGA DESC;
";
            using (var con = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddRange(parameters.ToArray());
                using (var adapter = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    // Añadir columna de PLACA y popularla en todas las filas
                    if (!dt.Columns.Contains("PLACA"))
                        dt.Columns.Add("PLACA", typeof(string));

                    foreach (DataRow row in dt.Rows)
                        row["PLACA"] = _placaCamion ?? string.Empty;

                    // Mostrar PLACA como primera columna para visibilidad
                    dt.Columns["PLACA"].SetOrdinal(0);

                    dgv.DataSource = dt;
                }
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            txtItems.Text = string.Empty;
            dgv.DataSource = null;
        }

        private void btnExportarCsv_Click(object sender, EventArgs e)
        {
            ExportarDgv(dgv, ExportFormat.Csv);
        }

        private void btnExportarExcel_Click(object sender, EventArgs e)
        {
            ExportarDgv(dgv, ExportFormat.Excel);
        }

        private int totalPages = 0;
        private int currentPage = 0;
        private System.Drawing.Printing.PrintDocument printDocument = null;
        private List<string> columnasParaImprimir = new List<string>();
        private DataTable dataParaImprimir = null;

        private void btnImprimirEtiquetado_Click(object sender, EventArgs e)
        {
            if (dgv.DataSource == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para imprimir.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            columnasParaImprimir.Clear();
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (col.Visible)
                    columnasParaImprimir.Add(col.HeaderText);
            }

            dataParaImprimir = ((DataTable)dgv.DataSource).Copy();

            printDocument = new System.Drawing.Printing.PrintDocument();
            printDocument.PrintPage += PrintDocument_PrintPage;
            printDocument.BeginPrint += PrintDocument_BeginPrint;

            PrintDialog printDialog = new PrintDialog();
            printDialog.Document = printDocument;
            printDialog.UseEXDialog = true;

            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.Print();
            }
        }

        private void PrintDocument_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            currentPage = 0;
            int rowsPerPage = 25;
            totalPages = (int)Math.Ceiling(dataParaImprimir.Rows.Count / (double)rowsPerPage);
        }

        private void PrintDocument_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int leftMargin = e.MarginBounds.Left;
            int topMargin = e.MarginBounds.Top;
            int width = e.MarginBounds.Width;
            int height = e.MarginBounds.Height;

            Font titleFont = new Font("Arial", 14, FontStyle.Bold);
            Font headerFont = new Font("Arial", 10, FontStyle.Bold);
            Font textFont = new Font("Arial", 9);

            string title = "Reporte de Items en Bodega";
            e.Graphics.DrawString(title, titleFont, Brushes.Black, leftMargin + (width / 2) - (e.Graphics.MeasureString(title, titleFont).Width / 2), topMargin);

            string dateTime = "Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            e.Graphics.DrawString(dateTime, textFont, Brushes.Black, leftMargin + width - e.Graphics.MeasureString(dateTime, textFont).Width, topMargin);

            int currentY = topMargin + (int)titleFont.GetHeight() + 40;

            int numColumns = columnasParaImprimir.Count;
            int[] columnWidths = new int[numColumns];
            int totalWidth = 0;
            for (int i = 0; i < numColumns; i++)
            {
                columnWidths[i] = width / numColumns;
                totalWidth += columnWidths[i];
            }
            columnWidths[numColumns - 1] += width - totalWidth;

            int currentX = leftMargin;
            for (int i = 0; i < numColumns; i++)
            {
                Rectangle cellRect = new Rectangle(currentX, currentY, columnWidths[i], (int)headerFont.GetHeight() + 5);
                e.Graphics.FillRectangle(Brushes.LightGray, cellRect);
                e.Graphics.DrawRectangle(Pens.Black, cellRect);
                e.Graphics.DrawString(columnasParaImprimir[i], headerFont, Brushes.Black, cellRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                currentX += columnWidths[i];
            }
            currentY += (int)headerFont.GetHeight() + 5;

            int rowsPerPage = 25;
            int startRow = currentPage * rowsPerPage;
            int endRow = Math.Min(startRow + rowsPerPage, dataParaImprimir.Rows.Count);

            for (int rowIndex = startRow; rowIndex < endRow; rowIndex++)
            {
                DataRow row = dataParaImprimir.Rows[rowIndex];
                currentX = leftMargin;
                for (int colIndex = 0; colIndex < numColumns; colIndex++)
                {
                    string cellValue = "";
                    if (colIndex < dataParaImprimir.Columns.Count)
                    {
                        object value = row[colIndex];
                        cellValue = value == null ? "" : value.ToString();
                    }
                    Rectangle cellRect = new Rectangle(currentX, currentY, columnWidths[colIndex], (int)textFont.GetHeight() + 5);
                    e.Graphics.DrawRectangle(Pens.Black, cellRect);
                    e.Graphics.DrawString(cellValue, textFont, Brushes.Black, cellRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    currentX += columnWidths[colIndex];
                }
                currentY += (int)textFont.GetHeight() + 5;
                if (currentY + (int)textFont.GetHeight() > e.MarginBounds.Bottom)
                {
                    break;
                }
            }

            currentPage++;
            string pageInfo = $"Página {currentPage} de {totalPages}";
            e.Graphics.DrawString(pageInfo, textFont, Brushes.Black, leftMargin + width - e.Graphics.MeasureString(pageInfo, textFont).Width, e.MarginBounds.Bottom - (int)textFont.GetHeight());
            e.HasMorePages = currentPage < totalPages;
        }

        public enum ExportFormat { Csv, Excel }
        public void ExportarDgv(DataGridView dgv, ExportFormat format)
        {
            if (dgv.DataSource == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para exportar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            using (var sfd = new SaveFileDialog { Filter = format == ExportFormat.Csv ? "CSV files (*.csv)|*.csv" : "Excel files (*.xlsx)|*.xlsx", FileName = format == ExportFormat.Csv ? "ItemsExport.csv" : "ItemsExport.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    {
                        if (format == ExportFormat.Csv)
                        {
                            DataGridViewExporter.ExportToCsv(dgv, sfd.FileName);
                        }
                        else
                        {
                            DataGridViewExporter.ExportToExcel(dgv, sfd.FileName);
                        }
                        MessageBox.Show("Datos exportados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }
    }
}
