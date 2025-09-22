using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Models;
using ALISTAMIENTO_IE.Services;
using ALISTAMIENTO_IE.Utils;
using DocumentFormat.OpenXml.Bibliography;
using ExcelDataReader;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace ALISTAMIENTO_IE
{
    public partial class ALISTAR_CAMION : Form
    {
        private readonly AlistamientoService alistamientoService;
        private readonly DetalleCamionXDiaService _detalleCamionXDiaService;
        private readonly AlistamientoEtiquetaService _alistamientoEtiquetaService; // reporte
        private readonly CargueMasivoService _cargueMasivoService;
        private List<CamionDetallesDTO> _camiones;
        private readonly TimerTurnos _turnoTimerManager; // Manejador de Timer y Turnos
        private readonly System.Windows.Forms.Timer _timer;
        private System.Windows.Forms.Timer _cooldownTimer;
        private bool _canClick;

        private List<MovimientoDocumentoDto> listaNormal = new();
        private List<GrupoMovimientosDto> listaAgrupada = new();

        public ALISTAR_CAMION()
        {
            InitializeComponent();

            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;


            _detalleCamionXDiaService = new DetalleCamionXDiaService();
            alistamientoService = new AlistamientoService();
            _alistamientoEtiquetaService = new AlistamientoEtiquetaService();
            _turnoTimerManager = new TimerTurnos(this); // inicializar el timer
            _cargueMasivoService = new CargueMasivoService();

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
                RECARGAR.Enabled = true;
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

                dgvResumen.DataSource = DataGridViewExporter.ConvertDynamicToDataTable(data);
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
                        item.BackColor = Color.Orange;
                        item.ForeColor = Color.White;
                        break;
                    case "SIN_ALISTAR":
                        item.BackColor = Color.LightGray;
                        break;
                    case "EN_PROCESO":
                        item.BackColor = Color.LightBlue;
                        break;
                    case "ALISTADO":
                        item.BackColor = Color.LightGreen;
                        break;
                    case "ANULADO":
                        item.BackColor = Color.LightCoral;
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
            int codCamion = selectedItem.Tag is int tag ? tag : 0;

            // Recuperar los valores de cada columna (SubItems)
            string placas = selectedItem.SubItems[0].Text;     // Placas
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

            if (codCamion > 0)
            {
                MostrarItemsDeCamion(codCamion);
                btnAlistar.Visible = true;

                lblTituloCamion.Text = $"CAMIÓN - {placas}";
                lblFechaValor.Text = fecha.ToString("dd/MM/yyyy"); // lo muestras en formato lindo
            }
        }





        private void MostrarItemsDeCamion(int codCamion)
        {
            List<ItemsDetalleDTO> items = new List<ItemsDetalleDTO>(alistamientoService.ObtenerItemsPorAlistarCamion(codCamion));
            dgvItems.DataSource = items;
            if (dgvItems.Columns.Count > 0)
            {
                dgvItems.Columns["Item"].HeaderText = "ITEM";
                dgvItems.Columns["Descripcion"].HeaderText = "Descripción";
                dgvItems.Columns["CantidadPlanificada"].HeaderText = "Cantidad";
            }
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

        private void RECARGAR_Click(object sender, EventArgs e)
        {
            if (!_canClick) return;  // Ignora si está en cooldown

            _canClick = false;
            RECARGAR.Enabled = false;

            CargarUI();

            // Inicia el cooldown
            _cooldownTimer.Start();
        }

        private async void btnCargarArchivo_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog
            {
                Title = "Selecciona un archivo de Excel",
                Filter = "Archivos de Excel|*.xlsx;*.xls;*.xlsb"
            };

            if (ofd.ShowDialog() != DialogResult.OK) return;

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
                return;
            }

            DataTable dt = ds.Tables[0];

            // Listas de salida
            listaNormal = new List<MovimientoDocumentoDto>();

            // Lista auxiliar para agrupar (lleva claves + DTO)
            var itemsParaAgrupar = new List<(DateTime Fecha, string EmpresaTransporte, long CodCamion, long CodConductor, MovimientoDocumentoDto Movimiento)>();

            // --- Recorrido filas ---
            foreach (DataRow fila in dt.Rows)
            {
                string empresa = fila["EMPRESA"]?.ToString()?.Trim() ?? "";
                string tipoDocumento = fila["TIPO DOCUMENTO"]?.ToString()?.Trim() ?? "";
                string idDocumentoTxt = fila["ID DOCUMENTO"]?.ToString()?.Trim() ?? "";
                string ciaTransporteTxt = fila["NIT CIA TRANSPORTE"]?.ToString()?.Trim() ?? "";
                string codConductorTxt = fila["COD_CONDUCTOR"]?.ToString()?.Trim() ?? "";
                string item = fila["ITEM"]?.ToString()?.Trim() ?? "";
                string codCamionTxt = fila["COD CAMION"]?.ToString()?.Trim() ?? "";
                string puntoEnvio = fila["PUNTO ENVIO"]?.ToString()?.Trim() ?? "";
                string cantidadTxt = fila["CANTIDAD"]?.ToString()?.Trim() ?? "";

                // Validaciones mínimas
                if (!int.TryParse(idDocumentoTxt, out var idDocumento))
                {
                    MessageBox.Show($"ID DOCUMENTO inválido: {idDocumentoTxt}");
                    continue;
                }
                if (!int.TryParse(ciaTransporteTxt, out var rowIdTransporte))
                {
                    MessageBox.Show($"NIT CIA TRANSPORTE inválido: {ciaTransporteTxt}");
                    continue;
                }
                if (!long.TryParse(codConductorTxt, out var codConductorLong))
                {
                    MessageBox.Show($"COD_CONDUCTOR inválido: {codConductorTxt}");
                    continue;
                }
                if (!long.TryParse(codCamionTxt, out var codCamionLong))
                {
                    MessageBox.Show($"COD CAMION inválido: {codCamionTxt}");
                    continue;
                }

                // Llamados a servicios
                var documento = await _cargueMasivoService.ObtenerDocumentoContableAsync(empresa, tipoDocumento, idDocumento);
                var tercero = await _cargueMasivoService.ObtenerTerceroPorRowIdAsync(rowIdTransporte); // tiene f200_id y f200_razon_social
                var conductor = await _cargueMasivoService.ObtenerConductorPorCodigoAsync(codConductorLong);
                var camion = await _cargueMasivoService.ObtenerCamionPorCodigoAsync(codCamionLong);
                var movsDoc = await _cargueMasivoService.ObtenerMovimientosPorConsecutivoAsync(idDocumento); // TTS por defecto

                if (documento == null)
                {
                    MessageBox.Show($"Documento no encontrado: {tipoDocumento}/{idDocumento}");
                    continue;
                }
                if (tercero == null)
                {
                    MessageBox.Show($"Compañía de transporte no encontrada (rowid): {rowIdTransporte}");
                    continue;
                }
                if (conductor == null)
                {
                    MessageBox.Show($"Conductor no encontrado: {codConductorLong}");
                    continue;
                }
                if (camion == null)
                {
                    MessageBox.Show($"Camión no encontrado: {codCamionLong}");
                    continue;
                }

                // Nombre visible de la empresa de transporte (usa lo que prefieras)
                string empresaTransporte = tercero.f200_id;

                // Armar lista normal y auxiliar de agrupación
                foreach (var m in movsDoc)
                {
                    var movimiento = new MovimientoDocumentoDto
                    {
                        FECHA = m.FECHA,
                        NUM_DOCUMENTO = documento.Documento,
                        ESTADO = m.ESTADO,
                        NOMBRE_CONDUCTOR = conductor.NOMBRES ?? "",
                        BOD_SALIDA = m.BOD_SALIDA,
                        BOD_ENTRADA = m.BOD_ENTRADA,
                        ITEM_RESUMEN = m.ITEM_RESUMEN,
                        CANT_SALDO = m.CANT_SALDO,
                        NOTAS_DEL_DOCTO = m.NOTAS_DEL_DOCTO
                    };

                    listaNormal.Add(movimiento);

                    itemsParaAgrupar.Add((m.FECHA.Date, empresaTransporte, camion.COD_CAMION, conductor.COD_CONDUCTOR, movimiento));
                }
            }

            // --- Agrupar por Fecha -> EmpresaTransporte -> CodCamion -> CodConductor ---
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
               .OrderBy(g => g.Fecha)
               .ThenBy(g => g.EmpresaTransporte)
               .ThenBy(g => g.CodCamion)
               .ThenBy(g => g.CodConductor)
               .ToList();

            // Muestra la lista "normal" en tu grid
            dtgCargueMasivo.DataSource = listaNormal;
            dtgCargueMasivo.AutoResizeColumns();

            // Si tienes otro grid para la agrupada, podrías hacer:
            dtgAgrupada.DataSource = listaAgrupada;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Llamada al service
                int filas = await _cargueMasivoService.GuardarCamionDiaYDetallesAsync(
                    listaAgrupada,
                    estadoCabecera: "C",     // Estado de la cabecera (ej: 'C' = creado)
                    estadoDetalle: "C",      // Estado de los detalles
                    unidadMedidaDefault: "UND" // Puedes cambiarlo a "KLS" o null según tu lógica
                );

                MessageBox.Show($"Se guardaron {filas} registros (cabeceras + detalles).", "Éxito");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar en BD: {ex.Message}");
            }
        }

        private void ALISTAR_CAMION_Load(object sender, EventArgs e)
        {

        }
    }
}
