using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Interfaces;

namespace ALISTAMIENTO_IE.Forms
{
    public partial class EtqsEliminadasEnAlistamiento : Form
    {
        // 1. Uso de la Interfaz (Desacoplamiento)
        private readonly IEliminacionAlistamientoEtiquetaService _elimService;
        private readonly int _idAlistamiento;

        public EtqsEliminadasEnAlistamiento(int idAlistamiento, IEliminacionAlistamientoEtiquetaService elimService)
        {
            InitializeComponent();
            _idAlistamiento = idAlistamiento;
            _elimService = elimService;

            // 3. Inicialización de eventos en el constructor, ¡como pediste!
            lstEtiquetasEliminadas.SelectedIndexChanged += lstEtiquetasEliminadas_SelectedIndexChanged;
            btnRevertir.Click += btnRevertir_Click;
            btnRevertir.Enabled = false; // Deshabilitar por defecto
        }

        private async void EtqsEliminadasEnAlistamiento_Load(object sender, EventArgs e)
        {
            await CargarListaAsync();
        }

        // 4. CargarListaAsync optimizado (usa el DTO)
        private async Task CargarListaAsync()
        {
            // Ya no necesitas ObtenerEtiquetaPorIdAsync, el servicio lo resolvió con el JOIN
            var eliminadas = await _elimService.ObtenerEliminacionesPorAlistamientoConEtiquetaAsync(_idAlistamiento);

            lstEtiquetasEliminadas.Items.Clear();
            foreach (var reg in eliminadas)
            {
                var item = new ListViewItem(new[]
                {
                    reg.EtiquetaTexto ?? "(desconocida)", // Ya resuelta en el DTO
                    reg.FechaEliminacion.ToString("yyyy-MM-dd HH:mm")
                })
                {
                    Tag = reg // Guardamos el DTO
                };
                lstEtiquetasEliminadas.Items.Add(item);
            }
        }

        // 5. Lógica de habilitación del botón (UX)
        private void lstEtiquetasEliminadas_SelectedIndexChanged(object? sender, EventArgs e)
        {
            // Habilitar solo si se selecciona exactamente un ítem.
            btnRevertir.Enabled = lstEtiquetasEliminadas.SelectedItems.Count == 1;

            // Si deseleccionamos, limpiamos los detalles
            if (lstEtiquetasEliminadas.SelectedItems.Count == 0)
            {
                txtObservaciones.Text = string.Empty;
                dgvInfoEliminacion.DataSource = null;
            }
        }

        // 6. Lógica de doble click (ahora usa el DTO)
        private void lstEtiquetasEliminadas_DoubleClick(object sender, EventArgs e)
        {
            if (lstEtiquetasEliminadas.SelectedItems.Count == 0) return;

            var reg = lstEtiquetasEliminadas.SelectedItems[0].Tag as EliminacionAlistamientoEtiquetaDTO;
            if (reg == null) return;

            // Cargar observaciones al textbox
            txtObservaciones.Text = reg.Observaciones ?? "(Sin observaciones)";

            // Cargar resto de info al grid 
            var data = new[]
            {
        new { Campo = "Etiqueta", Valor = reg.EtiquetaTexto },
        new { Campo = "FechaEliminacion", Valor = reg.FechaEliminacion.ToString("yyyy-MM-dd HH:mm") },
        // <<< CAMBIO AQUÍ >>> Ahora mostramos el nombre
        new { Campo = "Usuario Elimina", Valor = reg.NombreUsuarioElimina },
    };
            dgvInfoEliminacion.DataSource = data;
        }

        // 7. Evento de Reversión (Orquestación)
        private async void btnRevertir_Click(object? sender, EventArgs e)
        {
            if (lstEtiquetasEliminadas.SelectedItems.Count == 0) return;

            var item = lstEtiquetasEliminadas.SelectedItems[0];
            var regAuditoria = item.Tag as EliminacionAlistamientoEtiquetaDTO;

            if (regAuditoria == null) return;

            // ... (Lógica de confirmación)

            try
            {
                // **IMPORTANTE:** Obtén el ID de usuario real de tu sistema de autenticación
                int idUsuarioActual = 123;

                await _elimService.RevertirEliminacionAsync(regAuditoria.IdEliminacionAlistamientoEtiqueta, idUsuarioActual);

                MessageBox.Show("Reversión completada. El ítem fue devuelto al Alistamiento.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await CargarListaAsync();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al revertir: {ex.Message}", "Error de Transacción", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}