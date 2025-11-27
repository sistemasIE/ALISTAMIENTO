using ALISTAMIENTO_IE.Interfaces;
using System.Data;

namespace ALISTAMIENTO_IE.Forms
{
    public partial class FormDetalleAlistamiento : Form
    {
        private readonly IAlistamientoEtiquetaService _alistamientoEtiquetaService;
        private readonly IAlistamientoService _alistamientoService;
        private readonly int _idAlistamiento;
        private readonly System.Windows.Forms.Timer _debounceTimer;

        public FormDetalleAlistamiento(int idAlistamiento,
                                       string placasCamion,
                                       IAlistamientoEtiquetaService alistamientoEtiquetaService,
                                       IAlistamientoService alistamientoService)
        {
            InitializeComponent();
            _idAlistamiento = idAlistamiento;
            _alistamientoEtiquetaService = alistamientoEtiquetaService;
            _alistamientoService = alistamientoService;

            _debounceTimer = new System.Windows.Forms.Timer { Interval = 350 };
            _debounceTimer.Tick += DebounceTimer_Tick;

            // Cambiar el título: 

            this.lblTitulo.Text += " " + placasCamion;

            txtBuscarItem.TextChanged += TxtBuscarItem_TextChanged;
        }

        private async void FormDetalleAlistamiento_Load(object sender, EventArgs e)
        {
            await CargarDatosAsync();
        }

        private async Task CargarDatosAsync(string? filtroItem = null)
        {
            var etiquetas = await _alistamientoEtiquetaService.ObtenerEtiquetasLeidas(_idAlistamiento);
            var lista = etiquetas.ToList();

            if (!string.IsNullOrWhiteSpace(filtroItem))
            {
                lista = lista.Where(e => e.ITEM.HasValue && e.ITEM.Value.ToString().Contains(filtroItem.Trim(), StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Bind al grid
            dgvEtiquetas.DataSource = lista;

            // Estadísticas
            lblTotalEtiquetas.Text = lista.Count.ToString();
            lblTotalItems.Text = lista.Where(e => e.ITEM.HasValue).Select(e => e.ITEM.Value).Distinct().Count().ToString();

            // Observaciones del alistamiento (sin depender del tipo concreto)
            var alistamientoObj = await _alistamientoService.ObtenerAlistamiento(_idAlistamiento);
            try
            {
                var prop = alistamientoObj?.GetType().GetProperty("Observaciones");
                txtObservaciones.Text = prop?.GetValue(alistamientoObj)?.ToString() ?? string.Empty;
            }
            catch
            {
                txtObservaciones.Text = string.Empty;
            }
        }

        private void TxtBuscarItem_TextChanged(object? sender, EventArgs e)
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();
        }

        private async void DebounceTimer_Tick(object? sender, EventArgs e)
        {
            _debounceTimer.Stop();
            await CargarDatosAsync(txtBuscarItem.Text);
        }
    }
}
