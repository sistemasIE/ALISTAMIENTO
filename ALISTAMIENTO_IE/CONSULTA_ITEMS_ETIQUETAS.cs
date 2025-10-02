using ALISTAMIENTO_IE.Utils;
using Microsoft.Data.SqlClient;
using System.Data;


namespace LECTURA_DE_BANDA
{
    public partial class CONSULTA_ITEMS_ETIQUETAS : Form
    {
        private readonly string? _placaCamion;
        private readonly PrinterService? _printerService;

        public CONSULTA_ITEMS_ETIQUETAS()
        {
            InitializeComponent();

            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;
            this._printerService = new PrinterService();

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



        private void btnImprimirEtiquetado_Click(object sender, EventArgs e)
        {
            if (dgv.DataSource == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para imprimir.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            // Generar PDF:
            string urlPdf = _printerService.GenerarReporteAlistamiento(dgv, dgv);

            // Imprimir:
            _printerService.ImprimirPDF(urlPdf);



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
