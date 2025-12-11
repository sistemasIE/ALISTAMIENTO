using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Services;
using ALISTAMIENTO_IE.Utils;
using Common.cache;
//using DocumentFormat.OpenXml.Spreadsheet;
using ExcelDataReader;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace ALISTAMIENTO_IE
{
    public partial class Menu : Form
    {
        private bool _canClick = true;
        private bool _hasUploaded = false;
        private Dictionary<string, string> _cacheItemsEquivalentes = new Dictionary<string, string>();
        private int codCamionSeleccionado;
        private List<CamionDetallesDTO> _camiones;
        private List<GrupoMovimientosDto> listaAgrupada = new();
        private List<long> codCamiones = new List<long>();
        private List<MovimientoDocumentoDto> listaNormal = new();
        private ListViewItem[] camionesSeleccionados;
        private List<ReporteTrazabilidadDto> _reporte;


        private readonly IServiceScopeFactory _scopeFactory;

        private readonly IAlistamientoService _alistamientoService;
        private readonly IPdfService _pdfService;
        private readonly ICamionXDiaService _camionXDiaService;
        private readonly IDetalleCamionXDiaService _detalleCamionXDiaService;
        private readonly IAlistamientoEtiquetaService _alistamientoEtiquetaService;
        private readonly IKardexService _kardexService;
        private readonly IDataGridViewExporter _dataGridViewExporter;
        private readonly CargueMasivoService _cargueMasivoService;
        private readonly IEmailService _emailService;


        private readonly System.Windows.Forms.Timer _timer;
        private readonly TimerTurnos _turnoTimerManager; // Manejador de Timer y Turnos
        private String placaCamionSeleccionado;
        private System.Windows.Forms.Timer _cooldownTimer;


        public Menu(
             IAlistamientoService alistamientoService,
             IPdfService pdfService,
             ICamionXDiaService camionXDiaService,
             IDetalleCamionXDiaService detalleCamionXDiaService,
             IAlistamientoEtiquetaService alistamientoEtiquetaService,
             IKardexService kardexService,
             IDataGridViewExporter dataGridViewExporter,
             CargueMasivoService cargueMasivoService,
             IEmailService emailService,
             IServiceScopeFactory scopeFactory
         )
        {
            InitializeComponent();
            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;

            // 1. Asignación de servicios (Adiós a los new())
            _alistamientoService = alistamientoService;
            _pdfService = pdfService;
            _camionXDiaService = camionXDiaService;
            _detalleCamionXDiaService = detalleCamionXDiaService;
            _alistamientoEtiquetaService = alistamientoEtiquetaService;
            _kardexService = kardexService;
            _dataGridViewExporter = dataGridViewExporter;
            _cargueMasivoService = cargueMasivoService;
            _emailService = emailService;
            _scopeFactory = scopeFactory; // Asignamos la fábrica

            _turnoTimerManager = new TimerTurnos(this);

            CargarUI();


            lvwListasCamiones.DoubleClick += lvwListasCamiones_DoubleClick;
            lvwListasCamiones.Click += lvwListasCamiones_DoubleClick;
            btnAlistar.Visible = true;

            // Timer para actualizar los camiones cada 5 minutos.
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 5 * 60 * 1000;
            _timer.Start();
            _timer.Tick += Timer_Tick;

            // Timer para recargar los camiones:
            _cooldownTimer = new System.Windows.Forms.Timer();
            _cooldownTimer.Interval = 5000; // 5 segundos
            _cooldownTimer.Tick += (s, e) =>
            {
                _canClick = true;
                btnRecargar.Enabled = true;
                _cooldownTimer.Stop();
            };


            // Reportes: cargar al entrar en la pestaña y al cambiar fecha y turno
            tabMain.SelectedIndexChanged += TabMain_SelectedIndexChanged;
            dtpFechaReporte.ValueChanged += DtpFechaReporte_ValueChanged;
            tbcReportes.SelectedIndexChanged += TbcTurnos_SelectedIndexChanged;

            // Validaciones:
            LimitarVistaPorUsuario();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            CargarUI();
        }


        private void CargarUI()
        {
            CargarCamiones();
        }


        private async void TabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabReportes)
            {
                // Al entrar a la pestaña de Reportes:
                await CargarReportesYTotales();
            }
        }

        private async void DtpFechaReporte_ValueChanged(object sender, EventArgs e)
        {
            // Al cambiar la fecha, cargar acorde al tab de turno seleccionado
            if (tabMain.SelectedTab == tabReportes)
            {
                await CargarReportesYTotales();
            }


        }

        private async void TbcTurnos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.SelectedTab == tabReportes)
            {
                // Al cambiar de turno dentro de la pestaña Reportes, cargar acorde al tab
                await CargarReportesYTotales();
            }
        }

        private async Task CargarReportesYTotales()
        {
            // Primero, determinar el filtro de turno
            string? turnoLike = null;
            if (tbcReportes.SelectedTab == tabTurno1)
            {
                turnoLike = "%TURNO1%";
            }
            else if (tbcReportes.SelectedTab == tabTurno2)
            {
                turnoLike = "%TURNO2%";
            }
            else if (tbcReportes.SelectedTab == tabTurno3)
            {
                turnoLike = "%TURNO3%";
            }
            // Si el tab es "TOTAL", turnoLike se mantiene en null, lo cual es correcto
            else if (tbcReportes.SelectedTab == tabAlistamientos)
            {
                // 1. Traer todos los alistamientos que se dieron ese día. 

                IEnumerable<CamionXDia> alistamientosEnDia = await _camionXDiaService.ObtenerCamionesDespachadosPorFecha(dtpFechaReporte.Value);

                // 2. Obtener los id's camiones

                List<long> codCamionesEnDia = alistamientosEnDia.Select(a => a.CodCamion).Distinct().ToList();
                List<int> listaInt = codCamionesEnDia.Select(x => (int)x).ToList();

                // Mostrar la cantidad de camiones obtenidos en el día (usar la lista local, no la variable de clase codCamiones)
                lblCamionesNumero.Text = codCamionesEnDia.Count.ToString();

                // 3. Obtener el Reporte:

                _reporte = await _alistamientoService.ObtenerReporteTrazabilidad(codCamionesEnDia.Select(x => (int)x).ToList());

                // Distintos camiones y placas
                var itemsCombo = _reporte
                    .GroupBy(r => new { r.CodCamion, r.Placas })
                    .Select(g => new
                    {
                        Id = g.Key.CodCamion,
                        Texto = $"{g.Key.Placas} (ID {g.Key.CodCamion})"
                    })
                    .ToList();

                cmbReporteFull.DisplayMember = "Texto";
                cmbReporteFull.ValueMember = "Id";
                cmbReporteFull.DataSource = itemsCombo;

                var unidadesEnConflicto = Math.Round(Math.Abs(_reporte.Sum(r => r.PlanVsAlistado + r.AlistadoVsDespachado)), 2);
                lblUnidadesTexto.Text = "Novedades";
                lblUnidadesPacas.Text = unidadesEnConflicto.ToString();

                dgvResumen.DataSource = _dataGridViewExporter.ConvertDynamicToDataTable(_reporte);



                return;

            }
            try
            {
                // Llamar al método para obtener los totales de pacas y camiones.
                var totales = await _alistamientoEtiquetaService.ObtenerTotalesReporte(dtpFechaReporte.Value, turnoLike);
                if (totales != null)
                {
                    lblUnidadesPacas.Text = totales.TotalUnidades.ToString();
                    lblCamionesNumero.Text = totales.TotalCamiones.ToString();
                }
                else
                {
                    // Si no hay datos, mostrar 0
                    lblUnidadesPacas.Text = "0";
                    lblCamionesNumero.Text = "0";
                }

                // Ahora, cargar el reporte detallado para el DataGridView
                IEnumerable<dynamic> data;
                if (turnoLike == null)
                {
                    // Cargar reporte para el tab "TOTAL"
                    data = await _alistamientoEtiquetaService.ObtenerReportePacasPorHora(dtpFechaReporte.Value);
                }
                else
                {
                    // Cargar reporte para un turno específico
                    data = await _alistamientoEtiquetaService.ObtenerReportePacasPorHoraPorTurno(dtpFechaReporte.Value, turnoLike);
                }

                dgvResumen.DataSource = _dataGridViewExporter.ConvertDynamicToDataTable(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cargando reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CargarCamiones()
        {

            List<CamionEnAlistamientoDTO> _camiones = _alistamientoService.ObtenerCamionesEnAlistamiento().ToList();
            lvwListasCamiones.Items.Clear();
            foreach (CamionEnAlistamientoDTO camion in _camiones)
            {
                ListViewItem item = new ListViewItem(new[]
                {
                    camion.Placas,
                    camion.Fecha.ToString("yyyy-MM-dd"),
                    camion.CantTotalPedido.ToString(),
                    camion.EstadoAlistamiento
                });
                // Guarda el CodCamion en Tag para fácil acceso
                item.Tag = camion.CodCamion;

                // Colorear las filas según el estado del alistamiento para mejor visualización
                switch (camion.EstadoAlistamiento)
                {
                    case "ALISTADO_INCOMPLETO":
                        item.BackColor = System.Drawing.Color.Orange;
                        item.ForeColor = System.Drawing.Color.White;
                        break;
                    case "SIN_ALISTAR":
                        item.BackColor = System.Drawing.Color.LightGray;
                        break;
                    case "EN_PROCESO":
                        item.BackColor = System.Drawing.Color.LightBlue;
                        break;
                    case "ALISTADO":
                        item.BackColor = System.Drawing.Color.LightGreen;
                        break;
                    case "ANULADO":
                        item.BackColor = System.Drawing.Color.LightCoral;
                        break;
                }

                lvwListasCamiones.Items.Add(item);
            }
        }

        private async void lvwListasCamiones_DoubleClick(object sender, EventArgs e)
        {

            // Si no se seleccionó nada
            if (lvwListasCamiones.SelectedItems.Count == 0)
            {
                dgvItems.DataSource = null;
                return;
            }


            ListViewItem selectedItem = lvwListasCamiones.SelectedItems[0];

            // Recuperar el CodCamion que se guardó en el Tag
            codCamionSeleccionado = selectedItem.Tag is int tag ? tag : 0;
            // Acceder al estado del alistamiento (columna índice 3)
            string estadoAlistamiento = selectedItem.SubItems[3].Text;


            // Recuperar los valores de cada columna (SubItems)
            placaCamionSeleccionado = selectedItem.SubItems[0].Text;     // Placas

            string fechaTexto = selectedItem.SubItems[1].Text; // Fecha en texto

            // Convertir a DateTime de forma segura
            DateTime fecha;
            if (!DateTime.TryParseExact(fechaTexto, "yyyy-MM-dd",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out fecha))
            {
                MessageBox.Show("La fecha seleccionada no es válida.");
                return;
            }

            else if (codCamionSeleccionado > 0)
            {

                if (estadoAlistamiento == "ALISTADO" || estadoAlistamiento == "ALISTADO_INCOMPLETO")
                {
                    await _alistamientoService.CargarCamionDia(codCamionSeleccionado, this.dgvItems);

                }
                else
                {

                    MostrarItemsDeCamion(codCamionSeleccionado);
                    btnAlistar.Visible = true;

                }
                // Muestra el camión en la UI
                lblTituloCamion.Text = $"CAMIÓN - {placaCamionSeleccionado}";
                // Se muestra la fecha
                lblFechaValor.Text = fecha.ToString("dd/MM/yyyy");
            }
        }

        private async void MostrarItemsDeCamion(int codCamion)
        {
            List<CamionItemsDto> items = new List<CamionItemsDto>(await _alistamientoService.ObtenerItemsPorAlistarCamion(codCamion));
            dgvItems.DataSource = items;
        }



        private int? GetSelectedCamionId()
        {
            if (lvwListasCamiones.SelectedItems.Count == 0)
                return null;
            var selectedItem = lvwListasCamiones.SelectedItems[0];
            return selectedItem.Tag is int codCamion ? codCamion : (int?)null;
        }

        private void btnAlistar_Click(object sender, EventArgs e)
        {
            // Obtiene el id del camión
            int? codCamion = GetSelectedCamionId();

            // Verificación
            if (codCamion == null || codCamion <= 0)
            {
                MessageBox.Show("Seleccione un camión para alistar.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Obtener el estado actual del camión seleccionado
            string estadoActual = GetSelectedCamionEstado();

            // Validar que el estado permita el alistamiento
            if (estadoActual == "EN_PROCESO")
            {
                MessageBox.Show("Este camión ya tiene un alistamiento en proceso. No se puede iniciar un nuevo alistamiento.",
                              "Estado No Válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (estadoActual == "ALISTADO")
            {
                MessageBox.Show("Este camión ya ha sido alistado completamente. No se puede volver a alistar.",
                              "Estado No Válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (estadoActual == "ANULADO")
            {
                MessageBox.Show("Este camión tiene un alistamiento anulado. No se puede alistar.",
                              "Estado No Válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Solo permitir alistamiento para estados: SIN_ALISTAR y ALISTADO_INCOMPLETO
            if (estadoActual != "SIN_ALISTAR" && estadoActual != "ALISTADO_INCOMPLETO")
            {
                MessageBox.Show($"No se puede alistar un camión con estado '{estadoActual}'.",
                              "Estado No Válido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Si llegamos aquí, el estado es válido para alistamiento
            string mensaje = estadoActual == "ALISTADO_INCOMPLETO"
                ? "Este camión tiene un alistamiento incompleto. ¿Desea CONTINUAR con el alistamiento?"
                : "¿Desea INICIAR el alistamiento de este camión?";

            var result = MessageBox.Show(mensaje, "Confirmar Alistamiento",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);


            if (result == DialogResult.Yes)
            {
                // IMPORTANTE: Pasar el estado original para que ALISTAMIENTO sepa cómo manejarlo
                // pero el formulario ALISTAMIENTO se encargará de cambiar el estado a EN_PROCESO internamente
                using (var scope = _scopeFactory.CreateScope())
                {
                    // ActivatorUtilities resuelve automáticamente todas las dependencias
                    // del constructor de ALISTAMIENTO (excepto los parámetros de contexto).
                    var formAlistamiento = ActivatorUtilities.CreateInstance<ALISTAMIENTO>(
                        scope.ServiceProvider,
                        codCamionSeleccionado,
                        estadoActual
                    );
                    formAlistamiento.ShowDialog(this);
                }

                // Recargar los camiones después de cerrar el formulario para reflejar cambios
                CargarCamiones();
            }
        }

        /// <summary>
        /// Obtiene el estado del alistamiento del camión seleccionado
        /// </summary>
        /// <returns>Estado del alistamiento del camión seleccionado</returns>
        private string GetSelectedCamionEstado()
        {
            if (lvwListasCamiones.SelectedItems.Count == 0)
                return string.Empty;

            var selectedItem = lvwListasCamiones.SelectedItems[0];
            // El estado está en la columna 3 (índice 3) del ListView
            return selectedItem.SubItems.Count > 3 ? selectedItem.SubItems[3].Text : string.Empty;
        }

        private void ALISTAR_CAMION_FormClosing(object sender, FormClosingEventArgs e)
        {
            _turnoTimerManager.Stop();
        }

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            if (!_canClick) return;  // Ignora si está en cooldown

            _canClick = false;
            btnRecargar.Enabled = false;

            CargarUI();

            // Inicia el cooldown
            _cooldownTimer.Start();
        }
        public static string? InputBox(string prompt, string title, string defaultValue = "")
        {
            using (Form form = new Form())
            using (Label label = new Label())
            using (TextBox textBox = new TextBox())
            using (Button buttonOk = new Button())
            using (Button buttonCancel = new Button())
            {
                // Configuración del formulario
                form.Text = title;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.FormBorderStyle = FormBorderStyle.FixedDialog;
                form.MinimizeBox = false;
                form.MaximizeBox = false;
                form.AutoSize = true;
                form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                form.MinimumSize = new Size(500, 200);
                form.BackColor = Color.White;
                form.Padding = new Padding(15, 15, 15, 60);

                // Configuración del label
                label.AutoSize = true;
                label.Text = prompt;
                label.Location = new Point(15, 15);
                label.MaximumSize = new Size(450, 0);
                label.Font = new Font("Segoe UI", 9.75f, FontStyle.Regular);
                label.ForeColor = Color.FromArgb(64, 64, 64);

                // Ajustar altura del label si es necesario
                form.Controls.Add(label);
                int labelHeight = label.Height;

                // Configuración del textBox
                textBox.Text = defaultValue;
                textBox.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
                textBox.Location = new Point(15, label.Bottom + 15);
                textBox.Size = new Size(450, 28);
                textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                textBox.BorderStyle = BorderStyle.FixedSingle;

                // Seleccionar todo el texto al abrir
                textBox.SelectAll();

                // Configuración de los botones
                buttonOk.Text = "OK";
                buttonOk.DialogResult = DialogResult.OK;
                buttonOk.Size = new Size(85, 32);
                buttonOk.Location = new Point(290, 110);
                buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonOk.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
                buttonOk.BackColor = Color.FromArgb(0, 120, 215);
                buttonOk.ForeColor = Color.White;
                buttonOk.FlatStyle = FlatStyle.Flat;
                buttonOk.FlatAppearance.BorderSize = 0;
                buttonOk.Cursor = Cursors.Hand;

                buttonCancel.Text = "Cancelar";
                buttonCancel.DialogResult = DialogResult.Cancel;
                buttonCancel.Size = new Size(85, 32);
                buttonCancel.Location = new Point(380, 110);
                buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
                buttonCancel.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
                buttonCancel.BackColor = Color.FromArgb(240, 240, 240);
                buttonCancel.ForeColor = Color.FromArgb(64, 64, 64);
                buttonCancel.FlatStyle = FlatStyle.Flat;
                buttonCancel.FlatAppearance.BorderColor = Color.FromArgb(200, 200, 200);
                buttonCancel.Cursor = Cursors.Hand;

                // Agregar controles al formulario
                form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });

                // Configurar botones por defecto
                form.AcceptButton = buttonOk;
                form.CancelButton = buttonCancel;

                // Enfocar el textBox al mostrar el formulario
                form.Shown += (s, e) => textBox.Focus();

                // Mostrar el diálogo
                DialogResult dialogResult = form.ShowDialog();

                return dialogResult == DialogResult.OK ? textBox.Text : null;
            }
        }

        private async void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            btnCargarArchivo.Enabled = false;
            lstErrores.Items.Clear();
            try
            {
                using var ofd = new OpenFileDialog
                {
                    Title = "Selecciona un archivo de Excel",
                    Filter = "Archivos de Excel|*.xlsx;*.xls;*.xlsb;*.xlsm"
                };

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    btnCargarArchivo.Enabled = true;
                    return;
                }

                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                using var stream = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = ExcelReaderFactory.CreateReader(stream);

                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
                };

                var ds = reader.AsDataSet(conf);
                if (ds.Tables.Count == 0)
                {
                    MessageBox.Show("El archivo no contiene hojas.");
                    btnCargarArchivo.Enabled = true;
                    return;
                }

                DataTable dt = ds.Tables[0];

                // --- Progreso: inicializar ---
                progressBar1.Visible = true;
                progressBar1.Value = 0;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = dt.Rows.Count;   // determinate
                lblProgreso.Visible = true;
                lblProgreso.Text = $"0 / {dt.Rows.Count}";

                listaNormal = new List<MovimientoDocumentoDto>();
                var itemsParaAgrupar = new List<(DateTime Fecha, string EmpresaTransporte, long CodCamion, long CodConductor, MovimientoDocumentoDto Movimiento)>();

                int procesadas = 0;

                var duplicados = dt.AsEnumerable()
                                .GroupBy(f => f["ID DOCUMENTO"]?.ToString()?.Trim())
                                .Where(g => !string.IsNullOrEmpty(g.Key) && g.Count() > 1)
                                .Select(g => g.Key)
                                .ToList();

                if (duplicados.Any())
                {
                    string ids = string.Join(", ", duplicados);
                    MessageBox.Show($"Los siguientes ID DOCUMENTO están repetidos: {ids}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                foreach (DataRow fila in dt.Rows)
                {
                    // === tu lógica actual ===
                    string fechaDespacho = fila["FECHA DESPACHO"]?.ToString()?.Trim() ?? "";
                    string empresa = fila["EMPRESA"]?.ToString()?.Trim() ?? "";
                    string tipoDocumento = fila["TIPO DOCUMENTO"]?.ToString()?.Trim() ?? "";
                    string idDocumentoTxt = fila["ID DOCUMENTO"]?.ToString()?.Trim() ?? "";
                    string ciaTransporteTxt = fila["ID CIA TRANSPORTE"]?.ToString()?.Trim() ?? "";
                    string codConductorTxt = fila["COD_CONDUCTOR"]?.ToString()?.Trim() ?? "";
                    string codCamionTxt = fila["COD_CAMION"]?.ToString()?.Trim() ?? "";

                    // --- Validaciones iniciales ---
                    if (!int.TryParse(idDocumentoTxt, out var idDocumento))
                    {
                        lstErrores.Items.Add($"Error: ID DOCUMENTO inválido ({idDocumentoTxt}) en tipo {tipoDocumento}, empresa {empresa}");
                        continue;
                    }

                    if (!int.TryParse(ciaTransporteTxt, out var rowIdTransporte))
                    {
                        lstErrores.Items.Add($"Error: ID CIA TRANSPORTE inválido ({ciaTransporteTxt}) en documento {tipoDocumento}/{idDocumento}");
                        continue;
                    }

                    if (!long.TryParse(codConductorTxt, out var codConductorLong))
                    {
                        lstErrores.Items.Add($"Error: COD_CONDUCTOR inválido ({codConductorTxt}) en documento {tipoDocumento}/{idDocumento}");
                        continue;
                    }

                    if (!long.TryParse(codCamionTxt, out var codCamionLong))
                    {
                        lstErrores.Items.Add($"Error: COD_CAMION inválido ({codCamionTxt}) en documento {tipoDocumento}/{idDocumento}");
                        continue;
                    }

                    var documento = await _cargueMasivoService.ObtenerDocumentoContableAsync(empresa, tipoDocumento, idDocumento);
                    var tercero = await _cargueMasivoService.ObtenerTerceroPorRowIdAsync(rowIdTransporte);
                    var conductor = await _cargueMasivoService.ObtenerConductorPorCodigoAsync(codConductorLong);
                    var camion = await _cargueMasivoService.ObtenerCamionPorCodigoAsync(codCamionLong);
                    var movsDoc = await _cargueMasivoService.ObtenerMovimientosPorConsecutivoAsync(idDocumento, tipoDocumento, int.Parse(empresa));





                    if (documento == null)
                    {
                        //MessageBox.Show("Documento no existe o Empresa Incorrecta: " + tipoDocumento + "/" + idDocumento);
                        lstErrores.Items.Add("Documento no existe o Empresa Incorrecta: " + tipoDocumento + "/" + idDocumento);
                        continue;
                    }

                    if (fechaDespacho == "")
                    {
                        //MessageBox.Show("Por favor ingrese la fecha de despacho del Documento " + tipoDocumento + "/" + idDocumento);
                        lstErrores.Items.Add("Por favor ingrese la fecha de despacho del Documento " + tipoDocumento + "/" + idDocumento);
                        continue;
                    }

                    if (tercero == null)
                    {

                        lstErrores.Items.Add("Compañía de transporte no existe o RowID incorrecto: " + tipoDocumento + " / " + idDocumento + "-->" + rowIdTransporte);
                        continue;
                    }
                    if (conductor == null)
                    {
                        lstErrores.Items.Add("Conductor no existe o código incorrecto: " + tipoDocumento + " / " + idDocumento + "-->" + codConductorLong);
                        continue;
                    }
                    if (camion == null)
                    {
                        lstErrores.Items.Add("Camión no existe o código incorrecto: " + tipoDocumento + " / " + idDocumento + "-->" + codCamionLong);
                        continue;
                    }

                    string empresaTransporte = tercero.f200_id.Trim();


                    var documentoDespachado = await _cargueMasivoService.ObtenerDocumentoDespachado(documento.Documento.ToString());

                    if (documentoDespachado != null)
                    {
                        //MessageBox.Show("El documento " + documentoDespachado.SECUENCIAL + " ya ha se ha programado anteriormente");
                        lstErrores.Items.Add("El documento " + documentoDespachado.SECUENCIAL + " ya ha se ha programado anteriormente");
                        continue;
                    }


                    foreach (var m in movsDoc)
                    {
                        m.FECHA = DateTime.Now;
                        var movimiento = new MovimientoDocumentoDto
                        {
                            FECHA = Convert.ToDateTime(fechaDespacho),
                            NUM_DOCUMENTO = documento.Documento,
                            TIPO_DOCUMENTO = tipoDocumento,
                            EMPRESA_TRANSPORTE = empresaTransporte,
                            ESTADO = m.ESTADO,
                            NOMBRE_CONDUCTOR = conductor.NOMBRES ?? "",
                            BOD_SALIDA = m.BOD_SALIDA,
                            BOD_ENTRADA = m.BOD_ENTRADA,
                            ITEM_RESUMEN = m.ITEM_RESUMEN,
                            CANT_SALDO = m.CANT_SALDO,
                            NOTAS_DEL_DOCTO = m.NOTAS_DEL_DOCTO
                        };
                        if (movimiento.BOD_SALIDA is not ("BODEGA INTEGRAL EMPAQUES" or "BODEGA TERCEROS IE" or "017" or "051C" or "050" or "045"))
                        {

                            //MessageBox.Show("Por favor revise la bodega de salida del Documento " + movimiento.NUM_DOCUMENTO);
                            lstErrores.Items.Add("Por favor revise la bodega de salida del Documento " + movimiento.NUM_DOCUMENTO);
                            continue;
                        }
                        if (int.Parse(empresa) == 1)
                        {
                            string itemOriginal = CargueMasivoService.ExtraerItemDesdeResumen(movimiento.ITEM_RESUMEN);
                            string itemEquivalente;

                            // Primero verificar si ya existe en el caché
                            if (_cacheItemsEquivalentes.ContainsKey(itemOriginal))
                            {
                                // Usar el item equivalente del caché
                                itemEquivalente = _cacheItemsEquivalentes[itemOriginal];
                            }
                            else
                            {
                                // Buscar en la base de datos
                                itemEquivalente = await _cargueMasivoService.SacaItemEquivalenteAsync(itemOriginal);

                                if (itemEquivalente == "N/A")
                                {
                                    string? nuevoItem = InputBox(
                                        $"El item {movimiento.ITEM_RESUMEN} no tiene equivalente en IE.\nPor favor ingrese el item equivalente:",
                                        "Item Equivalente", "");

                                    if (!string.IsNullOrWhiteSpace(nuevoItem))
                                    {
                                        // Guardar en caché para usar en los siguientes documentos
                                        _cacheItemsEquivalentes[itemOriginal] = nuevoItem;
                                        itemEquivalente = nuevoItem;

                                        string descripcion = await _cargueMasivoService.ObtenerDescripcionItemAsync(nuevoItem);
                                        movimiento.ITEM_RESUMEN = nuevoItem.TrimEnd() + "->" + descripcion + "-------" + "ITEM INGRESADO MANUALMENTE";
                                    }
                                    else
                                    {
                                        // Si el usuario cancela, mantener el item original
                                        movimiento.ITEM_RESUMEN = movimiento.ITEM_RESUMEN;
                                    }

                                    // 🔔 Enviar correo al GRUPO5 notificando que no tenía equivalente
                                    string asunto = $"ITEM SIN EQUIVALENTE: {itemOriginal}";
                                    string html = $@"
        <p><strong>Notificación automática del sistema de despacho</strong></p>
        <p>El item <strong>{itemOriginal + "-->" + await _cargueMasivoService.ObtenerDescripcionItemAsync(itemOriginal)}</strong> no tenía equivalente en IE.</p>
        {(string.IsNullOrWhiteSpace(nuevoItem)
                                            ? "<p>El usuario no ingresó ningún equivalente.</p>"
                                            : $"<p>El usuario ingresó manualmente el equivalente: <strong>{movimiento.ITEM_RESUMEN}</strong>.</p>")}
        <hr>
        <p><strong>Documento:</strong> {movimiento.NUM_DOCUMENTO}</p>
        <p><strong>Camión:</strong> {camion.PLACAS}</p>
        <p><strong>Conductor:</strong> {conductor.NOMBRES}</p>
        <p>Fecha: {DateTime.Now:dd/MM/yyyy HH:mm}</p>";

                                    // Obtener correos del grupo5
                                    var destinatarios = _cargueMasivoService.ejecuta_script(
                                        "select correos from [dbo].[GRUPOS_DISTRIBUCION_CORREO] where id_grupo='GRUPO5'");

                                    using (var client = new System.Net.Mail.SmtpClient("192.168.16.215"))
                                    {
                                        client.UseDefaultCredentials = false;
                                        client.Port = 2727;
                                        client.Credentials = new System.Net.NetworkCredential("cabana\\notificaciones", "Notifica@inte");
                                        client.EnableSsl = false;

                                        foreach (var correo in destinatarios)
                                        {
                                            using (var msg = new System.Net.Mail.MailMessage("notificaciones@integraldeempaques.com", correo))
                                            {
                                                msg.Subject = asunto;
                                                msg.IsBodyHtml = true;
                                                msg.BodyEncoding = System.Text.Encoding.UTF8;
                                                msg.Body = html;

                                                await client.SendMailAsync(msg);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // Guardar en caché el item equivalente encontrado
                                    _cacheItemsEquivalentes[itemOriginal] = itemEquivalente;

                                    movimiento.ITEM_RESUMEN = itemEquivalente.TrimEnd()
                                        + "->" + await _cargueMasivoService.ObtenerDescripcionItemAsync(itemEquivalente);
                                }

                            }

                            // Si el item ya estaba en caché, actualizarlo
                            // Si el item ya estaba en caché y fue ingresado manualmente, aplicar el formato
                            if (_cacheItemsEquivalentes.ContainsKey(itemOriginal))
                            {
                                string itemDelCache = _cacheItemsEquivalentes[itemOriginal];
                                string descripcion = await _cargueMasivoService.ObtenerDescripcionItemAsync(itemDelCache);

                                // Verificar si fue ingresado manualmente (comparando con el resultado de la BD)
                                string itemDeBD = await _cargueMasivoService.SacaItemEquivalenteAsync(itemOriginal);
                                bool esManual = itemDeBD == "N/A";

                                movimiento.ITEM_RESUMEN = itemDelCache.TrimEnd() + "->" + descripcion +
                                                         (esManual ? "-------ITEM INGRESADO MANUALMENTE" : "");
                            }
                        }
                        listaNormal.Add(movimiento);
                        itemsParaAgrupar.Add((m.FECHA.Date, empresaTransporte, camion.COD_CAMION, conductor.COD_CONDUCTOR, movimiento));
                    }

                    // --- Progreso: avanzar ---
                    procesadas++;
                    if (procesadas <= progressBar1.Maximum)
                        progressBar1.Value = procesadas;

                    lblProgreso.Text = $"{procesadas} / {dt.Rows.Count}";
                    await Task.Yield(); // permite refrescar UI entre iteraciones
                }

                // Agrupar y bindear
                listaAgrupada = itemsParaAgrupar
                   .GroupBy(x => new { x.Fecha, x.EmpresaTransporte, x.CodCamion, x.CodConductor })
                   .Select(g => new GrupoMovimientosDto
                   {
                       Fecha = g.Key.Fecha,
                       EmpresaTransporte = g.Key.EmpresaTransporte,
                       CodCamion = g.Key.CodCamion,
                       CodConductor = g.Key.CodConductor,
                       Movimientos = g.Select(x => x.Movimiento).ToList()
                   })
                   .OrderBy(g => g.Fecha).ThenBy(g => g.EmpresaTransporte).ThenBy(g => g.CodCamion).ThenBy(g => g.CodConductor)
                   .ToList();

                dtgCargueMasivo.DataSource = listaNormal;
                dtgCargueMasivo.AutoResizeColumns();
                dtgAgrupada.DataSource = listaAgrupada;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error procesando el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // --- Progreso: finalizar ---
                lblProgreso.Text = "";
                progressBar1.Visible = false;
                lblProgreso.Visible = false;
                btnCargarArchivo.Enabled = true; // ✅ SIEMPRE rehabilitar el botón
            }
        }

        private static string HtmlMessageBody(GrupoMovimientosDto grupo, string? placas, string? nombreConductor, string? docPrincipal)
        {
            var sb = new StringBuilder();

            sb.AppendLine($@"
<div style=""font-family: Segoe UI, Arial, sans-serif; font-size:13px;"">
  <p><strong>Programacion y Despacho camion:</strong> {System.Net.WebUtility.HtmlEncode(placas ?? $"CAMION {grupo.CodCamion}")} &nbsp;&nbsp;
     <strong>Conductor:</strong> {System.Net.WebUtility.HtmlEncode(nombreConductor ?? grupo.CodConductor.ToString())} &nbsp;&nbsp;
     <strong>De:</strong> &lt;&lt;{System.Net.WebUtility.HtmlEncode(docPrincipal ?? "")}&gt;&gt;</p>

  <table cellpadding=""6"" cellspacing=""0"" style=""border-collapse:collapse; border:1px solid #999; min-width:900px;"">
    <thead>
      <tr style=""background:#f0f6fb;"">
        <th style=""border:1px solid #999;"">FECHA</th>
        <th style=""border:1px solid #999;"">NUM DOCUMENTO</th>
        <th style=""border:1px solid #999;"">ESTADO</th>
        <th style=""border:1px solid #999;"">BOD. SALIDA</th>
        <th style=""border:1px solid #999;"">BOD. ENTRADA</th>
        <th style=""border:1px solid #999;"">ITEM RESUMEN</th>
        <th style=""border:1px solid #999;"">CANT. SALDO</th>
        <th style=""border:1px solid #999;"">NOTAS DEL DOCTO</th>
      </tr>
    </thead>
    <tbody>");

            foreach (var m in grupo.Movimientos)
            {
                sb.AppendLine($@"
      <tr>
        <td style=""border:1px solid #999;"">{m.FECHA:dd/MM/yyyy}</td>
        <td style=""border:1px solid #999;"">{System.Net.WebUtility.HtmlEncode(m.NUM_DOCUMENTO)}</td>
        <td style=""border:1px solid #999;"">{System.Net.WebUtility.HtmlEncode(m.ESTADO)}</td>
        <td style=""border:1px solid #999;"">{System.Net.WebUtility.HtmlEncode(m.BOD_SALIDA)}</td>
        <td style=""border:1px solid #999;"">{System.Net.WebUtility.HtmlEncode(m.BOD_ENTRADA)}</td>
        <td style=""border:1px solid #999;"">{System.Net.WebUtility.HtmlEncode(m.ITEM_RESUMEN)}</td>
        <td style=""border:1px solid #999; text-align:right;"">{m.CANT_SALDO:0.###}</td>
        <td style=""border:1px solid #999;"">{System.Net.WebUtility.HtmlEncode(m.NOTAS_DEL_DOCTO)}</td>
      </tr>");
            }

            sb.AppendLine(@"
    </tbody>
  </table>
</div>");

            return sb.ToString();
        }

        // Agregar este helper en la clase ALISTAR_CAMION:
        private void ClearUploadStateAfterSave()
        {
            try
            {
                // Mantener una copia visual (snapshot) del DataSource para que el usuario pueda ver los datos,
                // pero desvincularlos de las listas internas que se usan para guardar.
                if (dtgCargueMasivo.DataSource is IEnumerable<MovimientoDocumentoDto> listaVisual)
                {
                    dtgCargueMasivo.DataSource = listaVisual.Select(m => m).ToList(); // snapshot inmutable para la sesión
                }

                if (dtgAgrupada.DataSource is IEnumerable<GrupoMovimientosDto> grupoVisual)
                {
                    dtgAgrupada.DataSource = grupoVisual.Select(g => g).ToList();
                }
            }
            catch
            {
                // Si algo falla, no impedir la finalización. Dejamos el grid tal cual.
            }

            // Limpiar los parámetros/lists usados para el upload (ya no permitimos re-subir con los mismos objetos)
            listaNormal?.Clear();
            listaAgrupada?.Clear();
        }


        private async void btnCrearCamiones_Click(object sender, EventArgs e)
        {
            if (listaAgrupada is null || listaAgrupada.Count == 0)
            {
                MessageBox.Show("No hay grupos para guardar/enviar. Carga y agrupa primero.");
                return;
            }

            btnCrearCamiones.Enabled = false;

            try
            {
                // 1) Guardar
                int filas = await _cargueMasivoService.GuardarCamionDiaYDetallesAsync(
                    listaAgrupada,
                    estadoCabecera: "C",
                    estadoDetalle: "C",
                    unidadMedidaDefault: "UND"
                );

                // 2) Enviar emails (uno por cada grupo)
                foreach (var grupo in listaAgrupada)
                {
                    // Obtener placas y nombre del conductor si quieres enriquecer el asunto
                    var camion = await _cargueMasivoService.ObtenerCamionPorCodigoAsync(grupo.CodCamion);
                    var placas = camion?.PLACAS;
                    var nombreConductor = grupo.Movimientos.FirstOrDefault()?.NOMBRE_CONDUCTOR ?? "";
                    var docPrincipal = grupo.Movimientos.FirstOrDefault()?.NUM_DOCUMENTO ?? "";
                    var tipoDocPrincipal = grupo.Movimientos.FirstOrDefault()?.TIPO_DOCUMENTO ?? "";
                    var empresaTransporte = grupo.Movimientos.FirstOrDefault()?.EMPRESA_TRANSPORTE ?? "";

                    string asunto = $"Programacion y Despacho camion: {placas ?? $"CAMION {grupo.CodCamion}"} " +
                                    $"Conductor: {nombreConductor} de: <<{docPrincipal}>>";

                    string html = HtmlMessageBody(grupo, placas, nombreConductor, docPrincipal);

                    // Reemplaza esta línea incorrecta:
                    // var destinatarios = ["jefedesistemas@integraldeempaques.com", "otro@correo.com"];

                    // Por la sintaxis correcta de inicialización de arrays en C#:
                    var destinatarios = new string[] { "jefedesistemas@integraldeempaques.com", "otro@correo.com" };

                    if (tipoDocPrincipal == "TTS")
                    {
                        destinatarios = _cargueMasivoService.ejecuta_script("select correos from [dbo].[GRUPOS_DISTRIBUCION_CORREO] where id_grupo='GRUPO4'");
                    }
                    else if (tipoDocPrincipal == "RMV")
                    {
                        string grupos = "";
                        if (empresaTransporte == "900859908")
                        {
                            grupos = "GRUPO1";//escobar
                        }
                        else if (empresaTransporte == "900745904")
                        {
                            grupos = "GRUPO2";//turbotrans
                        }
                        else if (empresaTransporte == "800090323")
                        {
                            grupos = "GRUPO4";//fidelizado
                        }
                        else
                        {
                            grupos = "";
                        }
                        destinatarios = _cargueMasivoService.ejecuta_script("select correos from [dbo].[GRUPOS_DISTRIBUCION_CORREO] where id_grupo='" + grupos + "'");
                    }
                    else
                    {
                        destinatarios = new string[] { "jefedesistemas@integraldeempaques.com", "desarrollador@integraldeempaques.com" };

                    }

                    using (var client = new System.Net.Mail.SmtpClient("192.168.16.215"))
                    {
                        client.UseDefaultCredentials = false;
                        client.Port = 2727;
                        client.Credentials = new System.Net.NetworkCredential("cabana\\notificaciones", "Notifica@inte");
                        client.EnableSsl = false;

                        foreach (var correo in destinatarios)
                        {
                            using (var msg = new System.Net.Mail.MailMessage("notificaciones@integraldeempaques.com", correo))
                            {
                                msg.Subject = asunto;
                                msg.IsBodyHtml = true;
                                msg.BodyEncoding = System.Text.Encoding.UTF8;
                                msg.Body = html;

                                await client.SendMailAsync(msg);
                            }
                        }
                    }


                }

                MessageBox.Show($"Se guardaron {filas} registros y se enviaron los correos.", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnCrearCamiones.Enabled = true;
                ClearUploadStateAfterSave();
            }
        }

        private void btnDescPlantilla_Click(object sender, EventArgs e)
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;
            string projectPath = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\"));
            string sourcePath = Path.Combine(projectPath, "public", "template", "PLANTILLA_CARGUE.xlsx");


            string destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "plantilla_file.xlsx");

            try
            {
                File.Copy(sourcePath, destinationPath, true);
                MessageBox.Show("Archivo copiado correctamente en: " + destinationPath, "Hecho", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al copiar el archivo : {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnImprimir_Click(object sender, EventArgs e)
        {
            var logs = new List<string>();

            foreach (ListViewItem camion in camionesSeleccionados)
            {
                var idCamionSeleccionado = camion.Tag;
                var placaDelCamion = camion.SubItems[0].Text;

                CamionXDia? cxd = await _camionXDiaService.GetByIdAsync(idCamionSeleccionado is int cod ? cod : 0);

                if (cxd == null)
                {
                    logs.Add($"❌ {placaDelCamion}: NO se encontró el camión.");
                    continue;
                }

                IEnumerable<CamionItemsDto> items = await _alistamientoService.ObtenerItemsPorAlistarCamion((int)cxd.CodCamion);

                var validItems = items.Select(x => x.Item).ToList();

                if (validItems.Count == 0)
                {
                    logs.Add($"⚠️ {placaDelCamion}: No tiene ítems válidos.");
                    continue;
                }

                var totales = _alistamientoService.CalcularTotalesReporte(items);

                DataTable dt1 = _dataGridViewExporter.ToDataTable(items);
                _alistamientoService.AgregarFilaTotalesADataTable(dt1, totales);

                DataTable dt2 = _kardexService.ObtenerDatosDeItems(validItems);

                List<string> titles = new()
        {
            $"PLACA: {placaDelCamion}",
            cxd.Fecha?.ToString("dd/MM/yyyy") ?? ""
        };

                _pdfService.Generate(dt1, dt2, titles, "C:\\temp\\reporte"+ placaDelCamion+".pdf");

                logs.Add($"✔️ {placaDelCamion}: PDF generado. " +
                         $"Pacas: {totales.TotalPacasEsperadas:N2}, " +
                         $"Kilos: {totales.TotalKilosEsperados:N2}");
            }

            // Mostrar solo UNA ventana al final
            string resultado = string.Join("\n", logs);
            MessageBox.Show($"Resultados de impresión:\n\n{resultado}",
                            "Resumen", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public void AjustarListas(ListBox Lista)
        {
            // Permite dibujar ítems con varias líneas
            Lista.DrawMode = DrawMode.OwnerDrawVariable;

            // Calcula automáticamente la altura según el texto
            Lista.MeasureItem += (s, ev) =>
            {
                if (ev.Index >= 0)
                {
                    string text = Lista.Items[ev.Index].ToString();
                    SizeF size = ev.Graphics.MeasureString(text, Lista.Font, Lista.Width);
                    ev.ItemHeight = (int)size.Height + 6; // un pequeño margen
                }
            };

            // Dibuja el texto dentro del ListBox con salto de línea
            Lista.DrawItem += (s, ev) =>
            {
                ev.DrawBackground();

                if (ev.Index >= 0)
                {
                    string text = Lista.Items[ev.Index].ToString();
                    ev.Graphics.DrawString(
                        text,
                        Lista.Font,
                        Brushes.Black,
                        ev.Bounds,
                        new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Near }
                    );
                }

                ev.DrawFocusRectangle();
            };
        }

        public void LimitarVistaPorUsuario()
        {
            string nombreApp = Assembly.GetEntryAssembly()?.GetName().Name;



                if (!UserLoginCache.TienePermisoLike($"Administrador - [{nombreApp}]"))
            {
                if (!UserLoginCache.TienePermisoLike($"Cargue Masivo - [{nombreApp}]"))
                    tabMain.TabPages.Remove(tabCargueMasivo);

                    tabMain.TabPages.Remove(tabReportes);
                tabMain.TabPages.Remove(tabAdmonCamiones);
                if (!UserLoginCache.TienePermisoLike($"Operador - [{nombreApp}]"))
                {
                    btnAlistar.Visible = false;
                    btnVerMas.Visible = false;
                    btnImprimir.Visible = false;
                }
            }



        }
        // Función para listar los camiones y ordenarlos.
        private async void ALISTAR_CAMION_Load(object sender, EventArgs e)
        {
            AjustarListas(lstErrores);
            AjustarListas(lstCamiones);
            // --- Al llenar el ListBox ---
            var result = await _camionXDiaService.GetByStatusAsync();
            lstCamiones.Items.Clear();
            codCamiones.Clear();

            foreach (var c in result.OrderByDescending(x => (DateTime)x.FECHA))
            {
                string fecha = ((DateTime)c.FECHA).ToString("dd/MM/yyyy");
                lstCamiones.Items.Add($"{c.PLACAS} ---> {fecha}");
                codCamiones.Add((long)c.COD_CAMION);
            }

        }

        private void lstCamiones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCamiones.SelectedIndex >= 0)
            {
                long codCamion = codCamiones[lstCamiones.SelectedIndex];
                string texto = lstCamiones.SelectedItem.ToString();

                var detalle = _detalleCamionXDiaService.ObtenerPorCodCamion((int)codCamion);
                dataGridView1.DataSource = detalle;
            }
        }
        public async void RefrescarListaCamiones()
        {
            var result = await _camionXDiaService.GetByStatusAsync();
            lstCamiones.Items.Clear();
            codCamiones.Clear();
            foreach (var c in result.OrderByDescending(x => (DateTime)x.FECHA))
            {
                string fecha = ((DateTime)c.FECHA).ToString("dd/MM/yyyy");
                lstCamiones.Items.Add($"{c.PLACAS} ---> {fecha}");
                codCamiones.Add((long)c.COD_CAMION);
            }

            dataGridView1.DataSource = null;
        }
        private async void btnCerrarCamion_Click(object sender, EventArgs e)
        {
            if (lstCamiones.SelectedIndex < 0)
            {
                MessageBox.Show("Selecciona un camión antes de anularlo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            long codCamion = codCamiones[lstCamiones.SelectedIndex];
            string texto = lstCamiones.GetItemText(lstCamiones.SelectedItem);

            var alistamiento = await _alistamientoEtiquetaService.ObtenerItemAlistados(Convert.ToInt32(codCamion));

            if (alistamiento.Count > 0)
            {
                MessageBox.Show("No se puede anular el camión porque tiene alistamientos asociados.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"¿Seguro que deseas anular el camión:\n{texto}?\n\nEsto también eliminará los documentos asociados.",
                "Confirmar anulación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm != DialogResult.Yes)
                return;

            // Solicita causa del cierre
            string causa = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingrese la causa del cierre o anulación del camión:",
                "Causa del cierre",
                ""
            );

            if (string.IsNullOrWhiteSpace(causa))
            {
                MessageBox.Show("Debe ingresar una causa para continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Ejecuta la anulación
            bool ok = await _cargueMasivoService.AnularCamionConDocumentosAsync(codCamion);

            if (ok)
            {
                MessageBox.Show("Camión anulado y documentos eliminados correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefrescarListaCamiones();

                // Enviar correo de notificación


                string[] destinatarios = new string[]
                {
            "jefedesistemas@integraldeempaques.com",
            "desarrollador@integraldeempaques.com"
                };

                destinatarios = _cargueMasivoService.ejecuta_script("select correos from [dbo].[GRUPOS_DISTRIBUCION_CORREO] where id_grupo='GRUPO4'");


                await _emailService.NotificarAnulacionCamionAsync(codCamion, texto, causa, destinatarios);
            }
            else
            {
                MessageBox.Show("Error al anular el camión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void btnVerMas_Click(object? sender, EventArgs e)
        {
            int? codCamion = GetSelectedCamionId();
            if (codCamion == null || codCamion <= 0)
            {
                MessageBox.Show("Seleccione un camión para ver más.", "Atención", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Placas del camión seleccionado
            var placas = placaCamionSeleccionado;
            if (string.IsNullOrWhiteSpace(placas) && lvwListasCamiones.SelectedItems.Count > 0)
            {
                placas = lvwListasCamiones.SelectedItems[0].SubItems[0].Text;
            }

            // Obtener alistamiento actual por camión
            var alistamiento = await _alistamientoService.ObtenerAlistamientoPorCamionDia(codCamion.Value);
            if (alistamiento == null || alistamiento.IdAlistamiento <= 0)
            {
                MessageBox.Show("No hay alistamiento para este camión.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var scope = _scopeFactory.CreateScope();
            var form = ActivatorUtilities.CreateInstance<ALISTAMIENTO_IE.Forms.FormDetalleAlistamiento>(
                scope.ServiceProvider,
                alistamiento.IdAlistamiento,
                placas
            );
            form.ShowDialog(this);
        }

        private void lvwListasCamiones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvwListasCamiones.SelectedItems.Count == 0)
            {
                camionesSeleccionados = Array.Empty<ListViewItem>();
                return;
            }

            camionesSeleccionados = lvwListasCamiones
                .SelectedItems
                .Cast<ListViewItem>()
                .ToArray();
        }

        private void cmbReporteFull_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_reporte == null || cmbReporteFull.SelectedValue == null)
                return;

            int idSeleccionado = (int)cmbReporteFull.SelectedValue;

            var filtrado = _reporte
                .Where(r => r.CodCamion == idSeleccionado)
                .ToList();

            var unidadesEnConflicto = Math.Round(Math.Abs(filtrado.Sum(r => r.PlanVsAlistado + r.AlistadoVsDespachado)), 1);
            lblUnidadesTexto.Text = "Novedades";
            lblUnidadesPacas.Text = unidadesEnConflicto.ToString();

            lblCamionesNumero.Text = "1";

            dgvResumen.DataSource = _dataGridViewExporter.ConvertDynamicToDataTable(filtrado);


        }

      
    }
}
