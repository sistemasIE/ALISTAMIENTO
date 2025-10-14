using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Services;
using ALISTAMIENTO_IE.Utils;
using System.Data;

public enum ExportFormat { Csv, Excel }

namespace LECTURA_DE_BANDA
{
    public partial class CONSULTA_ITEMS_ETIQUETAS : Form
    {
        private readonly AlistamientoService _alistamientoService;
        private readonly DataGridView? dgvAImprimir;
        private readonly DataTable info;
        private readonly IDataGridViewExporter _dataGridViewExporter;
        private readonly IPrinterService _printerService;
        private readonly ItemService _itemService;
        private readonly KardexService _kardexService;
        private readonly string? _placaCamion;
        public CONSULTA_ITEMS_ETIQUETAS()
        {
            InitializeComponent();

            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;
            this._printerService = new PrinterService();
            this._dataGridViewExporter = new DataGridViewExporter();
            this._kardexService = new KardexService();
            this._itemService = new ItemService();
        }


        public CONSULTA_ITEMS_ETIQUETAS(IEnumerable<string> items, string placaCamion, DataTable info) : this()
        {
            _placaCamion = placaCamion;

            if (items != null)
            {
                txtItems.Text = string.Join("\r\n", items);
                btnBuscarItems_Click(this, EventArgs.Empty);
                this.info = info;
            }
        }

        private void btnBuscarItems_Click(object sender, EventArgs e)
        {
            var validItems = _itemService.ParseItemsFromTextArea(txtItems.Text).ToList(); ;

            if (validItems.Count == 0)
            {
                MessageBox.Show("Ingrese al menos un ITEM válido.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataTable dt = _kardexService.ObtenerDatosDeItems(validItems);
            dgv.DataSource = dt;
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

        private async void btnImprimirEtiquetado_Click(object sender, EventArgs e)
        {
            if (dgv.DataSource == null || dgv.Rows.Count == 0)
            {
                MessageBox.Show("No hay datos para imprimir.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var pdfService = new QuestPDFService(new DataGridViewExporter());

            pdfService.Generate(info, dgv);

            MessageBox.Show("PDF generado exitosamente:");
            System.Diagnostics.Process.Start("explorer.exe");

        }

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
                            _dataGridViewExporter.ExportToCsv(dgv, sfd.FileName);
                        }
                        else
                        {
                            _dataGridViewExporter.ExportToExcel(dgv, sfd.FileName);
                        }
                        MessageBox.Show("Datos exportados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

    }
}
