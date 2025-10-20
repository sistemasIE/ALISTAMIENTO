using ALISTAMIENTO_IE.Services;

namespace ALISTAMIENTO_IE
{
    public partial class OBSERVACIONES : Form
    {
        private readonly int _idAlistamiento;
        private readonly string _accion;
        private readonly AlistamientoService _alistamientoService;
        public bool AlistamientoAnulado { get; private set; } = false;
        public bool AlistamientoIncompleto { get; private set; } = false;

        public OBSERVACIONES(int idAlistamiento, string accion)
        {
            InitializeComponent();
            _idAlistamiento = idAlistamiento;
            _accion = accion;
            _alistamientoService = new AlistamientoService();
            lblDinamico.Text = accion;
            btnAceptar.Click += btnAceptar_Click;
            btnCancelar.Click += btnCancelar_Click;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            string obs = txtObservaciones.Text.Trim();
            if (string.IsNullOrWhiteSpace(obs))
            {
                MessageBox.Show("Debe ingresar una observación para continuar.", "Observación requerida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Verificar si es para alistamiento incompleto o anulación
            bool esAlistamientoIncompleto = _accion.Contains("incompleto", StringComparison.OrdinalIgnoreCase);

            if (MessageBox.Show("¿Está seguro de Pausar el alistamiento?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {

                _alistamientoService.ActualizarAlistamiento(_idAlistamiento, "ALISTADO_INCOMPLETO", obs, DateTime.Now);
                AlistamientoAnulado = true;
                this.DialogResult = DialogResult.OK;
                this.Close();

            }

        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
