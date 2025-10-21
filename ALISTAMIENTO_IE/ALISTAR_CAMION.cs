using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Services;
using ALISTAMIENTO_IE.Utils;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace ALISTAMIENTO_IE
{
    public partial class ALISTAR_CAMION : Form
    {
        private readonly AlistamientoService alistamientoService;
        private readonly IPdfService _pdfService;
        private readonly DetalleCamionXDiaService _detalleCamionXDiaService;
        private readonly AlistamientoEtiquetaService _alistamientoEtiquetaService; // reporte
        private readonly KardexService _kardexService; // reporte
        private readonly IDataGridViewExporter _dataGridViewExporter; // reporte
        private readonly CargueMasivoService _cargueMasivoService;
        private List<CamionDetallesDTO> _camiones;
        private readonly TimerTurnos _turnoTimerManager; // Manejador de Timer y Turnos
        private readonly System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.Timer _cooldownTimer;
        private bool _canClick = true;

        private List<MovimientoDocumentoDto> listaNormal = new();
        private List<GrupoMovimientosDto> listaAgrupada = new();

        private int codCamionSeleccionado;
        private String placaCamionSeleccionado;


        public ALISTAR_CAMION()
        {
            InitializeComponent();

            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;


            _detalleCamionXDiaService = new DetalleCamionXDiaService();
            alistamientoService = new AlistamientoService();
            _alistamientoEtiquetaService = new AlistamientoEtiquetaService();
            _turnoTimerManager = new TimerTurnos(this); // inicializar el timer
            _cargueMasivoService = new CargueMasivoService();
            _dataGridViewExporter = new DataGridViewExporter();
            _kardexService = new KardexService();
            _pdfService = new QuestPDFService(_dataGridViewExporter);

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
            tbcTurnos.SelectedIndexChanged += TbcTurnos_SelectedIndexChanged;
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
            if (tbcTurnos.SelectedTab == tabTurno1)
            {
                turnoLike = "%TURNO1%";
            }
            else if (tbcTurnos.SelectedTab == tabTurno2)
            {
                turnoLike = "%TURNO2%";
            }
            else if (tbcTurnos.SelectedTab == tabTurno3)
            {
                turnoLike = "%TURNO3%";
            }
            // Si el tab es "TOTAL", turnoLike se mantiene en null, lo cual es correcto

            try
            {
                // Llamar al método para obtener los totales de pacas y camiones.
                var totales = await _alistamientoEtiquetaService.ObtenerTotalesReporte(dtpFechaReporte.Value, turnoLike);
                if (totales != null)
                {
                    lblUnidadesPacas.Text = totales.TotalPacas.ToString();
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

            List<CamionEnAlistamientoDTO> _camiones = alistamientoService.ObtenerCamionesEnAlistamiento().ToList();
            lvwListasCamiones.Items.Clear();
            foreach (CamionEnAlistamientoDTO camion in _camiones)
            {
                ListViewItem item = new ListViewItem(new[]
                {
                    camion.Placas,
                    camion.Fecha.ToString("yyyy-MM-dd"),
                    camion.CantTotalPedido.ToString(),
                    camion.EstadoAlistamiento // Agregar el estado del alistamiento
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

        private void lvwListasCamiones_DoubleClick(object sender, EventArgs e)
        {
            if (lvwListasCamiones.SelectedItems.Count == 0)
            {
                dgvItems.DataSource = null;
                return;
            }


            ListViewItem selectedItem = lvwListasCamiones.SelectedItems[0];

            // Recuperar el CodCamion que se guardó en el Tag
            codCamionSeleccionado = selectedItem.Tag is int tag ? tag : 0;

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

            if (codCamionSeleccionado > 0)
            {
                MostrarItemsDeCamion(codCamionSeleccionado);
                btnAlistar.Visible = true;

                lblTituloCamion.Text = $"CAMIÓN - {placaCamionSeleccionado}";
                lblFechaValor.Text = fecha.ToString("dd/MM/yyyy"); // lo muestras en formato lindo
            }
        }

        private async void MostrarItemsDeCamion(int codCamion)
        {
            List<CamionItemsDto> items = new List<CamionItemsDto>(await alistamientoService.ObtenerItemsPorAlistarCamion(codCamion));
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
            int? codCamion = GetSelectedCamionId();
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
                var alistamientoForm = new ALISTAMIENTO(codCamion.Value, estadoActual);
                alistamientoForm.ShowDialog(this);

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
            Form form = new Form();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.Text = title;

            label.Text = prompt;
            textBox.Text = defaultValue;

            buttonOk.Text = "OK";
            buttonCancel.Text = "Cancelar";
            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            // --- tamaños más cómodos ---
            form.ClientSize = new Size(600, 160);                 // ventana más ancha/alta
            label.SetBounds(12, 12, 576, 0);
            label.AutoSize = true;
            label.MaximumSize = new Size(576, 0);                 // permite que el texto haga wrap

            textBox.Font = new System.Drawing.Font(textBox.Font, FontStyle.Regular);
            textBox.Font = new System.Drawing.Font(textBox.Font.FontFamily, 11f); // fuente más grande
            textBox.SetBounds(12, 60, 576, 32);                   // textbox más alto y ancho
            textBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            buttonOk.SetBounds(440, 110, 75, 28);
            buttonCancel.SetBounds(521, 110, 75, 28);
            buttonOk.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            buttonCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            form.Controls.AddRange(new Control[] { label, textBox, buttonOk, buttonCancel });

            // ⚠️ Importante: no volver a recalcular el ClientSize con label.Right (encogía la caja)
            // form.ClientSize = new Size(Math.Max(300, label.Right + 10), form.ClientSize.Height);

            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MinimizeBox = false;
            form.MaximizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            DialogResult dialogResult = form.ShowDialog();
            return dialogResult == DialogResult.OK ? textBox.Text : null;
        }

        private async void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            btnCargarArchivo.Enabled = false;

            try
            {
                using var ofd = new OpenFileDialog
                {
                    Title = "Selecciona un archivo de Excel",
                    Filter = "Archivos de Excel|*.xlsx;*.xls;*.xlsb;*.xlsm"
                };

                if (ofd.ShowDialog() != DialogResult.OK)
                {
                    btnCargarArchivo.Enabled = true; // ✅ CORREGIDO: Rehabilitar el botón si se cancela
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
                    btnCargarArchivo.Enabled = true; // ✅ CORREGIDO: Rehabilitar el botón si hay error
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

                foreach (DataRow fila in dt.Rows)
                {
                    // === tu lógica actual ===
                    string empresa = fila["EMPRESA"]?.ToString()?.Trim() ?? "";
                    string tipoDocumento = fila["TIPO DOCUMENTO"]?.ToString()?.Trim() ?? "";
                    string idDocumentoTxt = fila["ID DOCUMENTO"]?.ToString()?.Trim() ?? "";
                    string ciaTransporteTxt = fila["ID CIA TRANSPORTE"]?.ToString()?.Trim() ?? "";
                    string codConductorTxt = fila["COD_CONDUCTOR"]?.ToString()?.Trim() ?? "";
                    string codCamionTxt = fila["COD_CAMION"]?.ToString()?.Trim() ?? "";

                    if (!int.TryParse(idDocumentoTxt, out var idDocumento)) { /*...*/ continue; }
                    if (!int.TryParse(ciaTransporteTxt, out var rowIdTransporte)) { /*...*/ continue; }
                    if (!long.TryParse(codConductorTxt, out var codConductorLong)) { /*...*/ continue; }
                    if (!long.TryParse(codCamionTxt, out var codCamionLong)) { /*...*/ continue; }

                    var documento = await _cargueMasivoService.ObtenerDocumentoContableAsync(empresa, tipoDocumento, idDocumento);
                    var tercero = await _cargueMasivoService.ObtenerTerceroPorRowIdAsync(rowIdTransporte);
                    var conductor = await _cargueMasivoService.ObtenerConductorPorCodigoAsync(codConductorLong);
                    var camion = await _cargueMasivoService.ObtenerCamionPorCodigoAsync(codCamionLong);
                    var movsDoc = await _cargueMasivoService.ObtenerMovimientosPorConsecutivoAsync(idDocumento, tipoDocumento, int.Parse(empresa));


                    if(documento == null)
                    {
                        MessageBox.Show("Documento no existe o Empresa Incorrecta: " + tipoDocumento + "/" + idDocumento);
                        return;
                    }


                    if (tercero == null || conductor == null || camion == null) { /*...*/ continue; }

                    string empresaTransporte = tercero.f200_id.Trim();


                    var documentoDespachado = await  _cargueMasivoService.ObtenerDocumentoDespachado(documento.Documento.ToString());

                    if(documentoDespachado != null)
                    {
                        MessageBox.Show("El documento " + documentoDespachado.SECUENCIAL + " ya ha se ha programado anteriormente");
                        return;
                    }

                    foreach (var m in movsDoc)
                    {

                        m.FECHA = DateTime.Now;

                        var movimiento = new MovimientoDocumentoDto
                        {
                            FECHA = m.FECHA,
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

                        if (int.Parse(empresa) == 1)
                        {
                            string itemEquivalente = await _cargueMasivoService.SacaItemEquivalenteAsync(
                                CargueMasivoService.ExtraerItemDesdeResumen(movimiento.ITEM_RESUMEN));

                            if (itemEquivalente == "N/A")
                            {
                                string? nuevoItem = InputBox(
                                    $"El item {movimiento.ITEM_RESUMEN} no tiene equivalente en IE.\nPor favor ingrese el item equivalente:",
                                    "Item Equivalente", "");

                                if (!string.IsNullOrWhiteSpace(nuevoItem))
                                {
                                    string descripcion = await _cargueMasivoService.ObtenerDescripcionItemAsync(nuevoItem);
                                    movimiento.ITEM_RESUMEN = nuevoItem.TrimEnd() + "->" + descripcion + "-------" + "ITEM INGRESADO MANUALMENTE";
                                }
                            }
                            else
                            {
                                movimiento.ITEM_RESUMEN = itemEquivalente.TrimEnd()
                                    + "->" + await _cargueMasivoService.ObtenerDescripcionItemAsync(itemEquivalente);
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
        private async void button1_Click(object sender, EventArgs e)
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

                    //if (tipoDocPrincipal == "TTS")
                    //{
                    //    destinatarios = _cargueMasivoService.ejecuta_script("select correos from [dbo].[GRUPOS_DISTRIBUCION_CORREO] where id_grupo='GRUPO4'");
                    //}
                    //else  if (tipoDocPrincipal == "RMV")
                    //{
                    //    string grupos ="";
                    //    if(empresaTransporte == "900859908")
                    //    {
                    //        grupos= "GRUPO1";//escobar
                    //    }
                    //    else if(empresaTransporte== "900745904")
                    //    {
                    //        grupos= "GRUPO2";//turbotrans
                    //    }
                    //    else if(empresaTransporte == "800090323")
                    //    {
                    //        grupos = "GRUPO4";//fidelizado
                    //    }
                    //    else
                    //    {
                    //        grupos = "";
                    //    }
                    //    destinatarios = _cargueMasivoService.ejecuta_script("select correos from [dbo].[GRUPOS_DISTRIBUCION_CORREO] where id_grupo='"+grupos+"'");
                    //}
                    //else
                    //{
                    //    destinatarios = new string[] { "jefedesistemas@integraldeempaques.com", "desarrollador@integraldeempaques.com" };

                    //}

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

        private async void button1_Click_1(object sender, EventArgs e)
        {
            // Obtener los ítems del camión
            IEnumerable<CamionItemsDto> items = await alistamientoService.ObtenerItemsPorAlistarCamion(codCamionSeleccionado);

            // Extraer códigos de ítems para el kardex
            var validItems = items.Select(x => x.Item).ToList();

            if (validItems.Count == 0)
            {
                MessageBox.Show("No hay ítems válidos para el camión seleccionado.",
                                "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calcular totales usando el servicio
            ReporteImpresionTotalesDto totales = alistamientoService.CalcularTotalesReporte(items);

            // Convertir items a DataTable
            DataTable dt1 = _dataGridViewExporter.ToDataTable(items);

            // Agregar fila de totales usando el servicio
            alistamientoService.AgregarFilaTotalesADataTable(dt1, totales);

            // Obtener datos del kardex
            DataTable dt2 = _kardexService.ObtenerDatosDeItems(validItems);

            // Generar PDF
            _pdfService.Generate(dt1, dt2, $"PLACA: {placaCamionSeleccionado}");

            // Mostrar mensaje de éxito con los totales
            MessageBox.Show($"PDF generado exitosamente.\n\nTotales:\n" +
                          $"Cantidad Total: {totales.TotalCantTotalPedido:N2}\n" +
                          $"Pacas Esperadas: {totales.TotalPacasEsperadas:N2}\n" +
                          $"Kilos Esperados: {totales.TotalKilosEsperados:N2}");
        }

    }
}
