namespace ALISTAMIENTO_IE.Forms
{
    public partial class FormOperarEtiquetas : Form
    {
        private readonly string[] etiquetasIniciales;
        private readonly List<Func<string[], bool>> validaciones;
        private readonly List<Func<string[], Task>> operaciones;

        // Nuevo título dinámico
        private readonly string tituloOperacion;

        public FormOperarEtiquetas(string tituloOperacion,
                                   string[] etiquetas,
                                   List<Func<string[], bool>> validaciones = null,
                                   List<Func<string[], Task>> operaciones = null)
        {
            InitializeComponent();
            this.tituloOperacion = tituloOperacion;
            etiquetasIniciales = etiquetas;
            this.validaciones = validaciones ?? new List<Func<string[], bool>>();
            this.operaciones = operaciones ?? new List<Func<string[], Task>>();
            ActualizarTitulo();
        }

        // Constructor viejo conservado para compatibilidad opcional
        public FormOperarEtiquetas(string[] etiquetas,
                                   List<Func<string[], bool>> validaciones = null,
                                   List<Func<string[], Task>> operaciones = null)
        {
            InitializeComponent();
            this.tituloOperacion = "OPERAR"; // valor por defecto
            etiquetasIniciales = etiquetas;
            this.validaciones = validaciones ?? new List<Func<string[], bool>>();
            this.operaciones = operaciones ?? new List<Func<string[], Task>>();
            ActualizarTitulo();
        }

        private void ActualizarTitulo()
        {
            var titulo = $"{tituloOperacion?.ToUpperInvariant()} ETIQUETAS";
            lblPlaca.Text = titulo;
            Text = titulo;
        }

        public void SetTituloOperacion(string nuevoTitulo)
        {
            if (!string.IsNullOrWhiteSpace(nuevoTitulo))
            {
                // El campo readonly no puede cambiarse, pero podemos actualizar visualmente.
                lblPlaca.Text = nuevoTitulo.ToUpperInvariant() + " ETIQUETAS";
                Text = lblPlaca.Text;
            }
        }

        private void FormOperarEtiquetas_Load(object sender, EventArgs e)
        {
            txtBoxEtiquetas.Text = string.Join(Environment.NewLine, etiquetasIniciales);
        }

        private async void btnEjecutar_Click(object sender, EventArgs e)
        {
            var etiquetas = txtBoxEtiquetas.Lines
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Trim())
                .ToArray();

            foreach (var validar in validaciones)
            {
                if (!validar(etiquetas))
                {
                    MessageBox.Show("Error en validación. Revisa las etiquetas.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            try
            {
                foreach (var operacion in operaciones)
                {
                    await operacion(etiquetas);
                }

                MessageBox.Show("Operación completada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error durante la operación: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

