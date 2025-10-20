using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Model;
using ALISTAMIENTO_IE.Models;
using ALISTAMIENTO_IE.Services;
using Common.cache;
using System.ComponentModel; // Added for BindingList
using System.Data;
using System.Media;
using Timer = System.Windows.Forms.Timer;

namespace ALISTAMIENTO_IE
{
    public partial class ALISTAMIENTO : Form
    {
        private readonly int _idCamion;
        private readonly string _estadoAlistamientoInicial;
        private readonly AlistamientoService _alistamientoService;
        private readonly AlistamientoEtiquetaService _alistamientoEtiquetaService;
        private readonly KardexService _kardexService;
        private EtiquetaService _etiquetaService;
        private EtiquetaLinerService _etiquetaLinerService;
        private EtiquetaRolloService _etiquetaRolloService; // ✅ NUEVO

        private List<string> ITEMS_PENDIENTES = new List<string>();
        private int _idAlistamiento;
        private Timer _timer;
        private DateTime _fechaInicio;
        private string _estadoAlistamiento = "EN_PROCESO";
        private readonly CamionService _camionService;
        private readonly Camion CAMION_ACTUAL;



        private readonly DataGridViewExporter _dataGridViewExporter = new DataGridViewExporter();
        // Cache en memoria para acelerar cálculos sin tocar DB
        private List<string> etiquetasLeidas = new List<string>();
        private Dictionary<int, int> _conteoPorItem = new Dictionary<int, int>();
        private BindingList<DTOs.EtiquetaLeidaDTO> _leidasBinding = new BindingList<DTOs.EtiquetaLeidaDTO>();

        public ALISTAMIENTO(int idCamion) : this(idCamion, "EN_PROCESO") { }

        public ALISTAMIENTO(int idCamion, string estado)
        {
            InitializeComponent();

            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;

            _idCamion = idCamion;
            _estadoAlistamientoInicial = estado;
            _estadoAlistamiento = estado;

            _camionService = new CamionService();
            _alistamientoService = new AlistamientoService();
            _alistamientoEtiquetaService = new AlistamientoEtiquetaService();
            _etiquetaService = new EtiquetaService();
            _etiquetaLinerService = new EtiquetaLinerService();
            _etiquetaRolloService = new EtiquetaRolloService(); // ✅ NUEVO
            _kardexService = new KardexService();
            this.FormClosing += ALISTAMIENTO_FormClosing;
            this.Load += ALISTAMIENTO_Load;
            btnBuscar.Click += btnBuscar_Click;

            CAMION_ACTUAL = _camionService.GetCamionByCamionXDiaId(idCamion);
            lblPlaca.Text = CAMION_ACTUAL != null ? $"PLACAS: " + CAMION_ACTUAL.PLACAS.ToString() : "Desconocida";
        }

        /// <summary>
        /// Verifica si todas las columnas restantes (PacasRestantes, KilosRestantes y MetrosRestantes) están en cero
        /// </summary>
        /// <returns>True si todas las columnas restantes están en cero, False en caso contrario</returns>
        private bool VerificarAlistamientoCompleto()
        {
            var lista = (List<InformacionCamionDTO>)dgvMain.DataSource;
            if (lista == null || !lista.Any())
                return true;

            return lista.All(item => item.PacasRestantes <= 0 && item.KilosRestantes <= 0 && item.MetrosRestantes <= 0);
        }

        /// <summary>
        /// Maneja el evento click del botón TERMINAR
        /// </summary>
        private async void BtnTerminar_Click(object sender, EventArgs e)
        {

            var res = MessageBox.Show("¡¿Desea TERMINAR el alistamiento?!", "Confirmar",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
                if (VerificarAlistamientoCompleto())
                {
                    // Alistamiento completo - actualizar estado a ALISTADO
                    _alistamientoService.ActualizarAlistamiento(_idAlistamiento, "ALISTADO", "Alistamiento completado exitosamente", DateTime.Now);
                    _estadoAlistamiento = "ALISTADO";
                    MessageBox.Show("Alistamiento completado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    // Alistamiento incompleto - preguntar si desea cerrar con observaciones
                    var result = MessageBox.Show(
                        "No se puede terminar el alistamiento porque hay algunas unidades que no han sido alistadas completamente.\n\n¿Desea cerrar el alistamiento con observaciones?",
                        "Alistamiento Incompleto",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        using (var obsForm = new OBSERVACIONES(_idAlistamiento, "Indique el motivo del alistamiento incompleto"))
                        {
                            var obsResult = obsForm.ShowDialog(this);
                            if (obsResult == DialogResult.OK && obsForm.AlistamientoIncompleto)
                            {
                                // El estado ya fue actualizado en el form de observaciones
                                _estadoAlistamiento = "ALISTADO_INCOMPLETO";
                                MessageBox.Show("Alistamiento cerrado como incompleto.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                    }
                }
        }

        private async void ALISTAMIENTO_Load(object sender, EventArgs e)
        {
            try
            {
                _fechaInicio = DateTime.Now;

                if (_estadoAlistamientoInicial == "ALISTADO_INCOMPLETO")
                {
                    // CASO ESPECIAL: Continuar un alistamiento incompleto
                    Alistamiento alistamientoIncompleto = _alistamientoService.ObtenerAlistamientoPorCodCamionYEstado(_idCamion, "ALISTADO_INCOMPLETO");

                    if (alistamientoIncompleto == null)
                    {
                        MessageBox.Show("Error: No se encontró un alistamiento en estado 'ALISTADO_INCOMPLETO' para este camión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    _idAlistamiento = alistamientoIncompleto.IdAlistamiento;
                    _fechaInicio = alistamientoIncompleto.FechaInicio;

                    // IMPORTANTE: Cambiar el estado a EN_PROCESO para poder continuar alistando
                    _alistamientoService.ActualizarAlistamiento(_idAlistamiento, "EN_PROCESO", "Continuando alistamiento incompleto", null);
                    _estadoAlistamiento = "EN_PROCESO";

                    // Cargar etiquetas ya leídas en caché
                    await CargarEtiquetasLeidasEnCache();
                }
                else if (_estadoAlistamientoInicial == "EN_PROCESO" || _estadoAlistamientoInicial == "SIN_ALISTAR")
                {
                    // Verificar si ya existe un alistamiento en proceso para este camión
                    var alistamientoExistente = _alistamientoService.ObtenerAlistamientoPorCodCamionYEstado(_idCamion, "EN_PROCESO");

                    if (alistamientoExistente != null)
                    {
                        // Usar el alistamiento existente
                        _idAlistamiento = alistamientoExistente.IdAlistamiento;
                        _fechaInicio = alistamientoExistente.FechaInicio;

                        // Cargar etiquetas ya leídas en caché
                        await CargarEtiquetasLeidasEnCache();
                    }
                    else
                    {
                        // Crear un nuevo alistamiento
                        _idAlistamiento = _alistamientoService.InsertarAlistamiento(_idCamion, UserLoginCache.IdUser);
                    }
                }
                else
                {
                    // Para otros estados (visualización únicamente)
                    var alistamientoExistente = _alistamientoService.ObtenerAlistamientoPorCodCamionYEstado(_idCamion, _estadoAlistamientoInicial);

                    if (alistamientoExistente == null)
                    {
                        MessageBox.Show($"No se encontró un alistamiento con estado '{_estadoAlistamientoInicial}' para este camión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    _idAlistamiento = alistamientoExistente.IdAlistamiento;
                    _fechaInicio = alistamientoExistente.FechaInicio;

                    // Deshabilitar controles para visualización únicamente
                    txtEtiqueta.Enabled = false;
                    btnTerminar.Enabled = false;

                    // Cargar etiquetas ya leídas en caché
                    await CargarEtiquetasLeidasEnCache();
                }

                // Inicializar timer
                _timer = new Timer { Interval = 10000 }; // 10 segundos
                _timer.Tick += Timer_Tick;
                _timer.Start();
                ActualizarTimer();

                // Cargar etiquetas leídas para la UI
                await CargarEtiquetasLeidas();

                // Construir resumen inicial
                await cargarDgvMain();

                // Recalcular cantidades alistadas basándose en las etiquetas ya leídas
                await RecalcularCantidadesAlistadas();

                // Obtener los ítems pendientes del camión
                IEnumerable<CamionItemsDto> items = await _alistamientoService.ObtenerItemsPorAlistarCamion(_idCamion);
                ITEMS_PENDIENTES = items.Select(i => i.Item.ToString()).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar el formulario de alistamiento:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        /// <summary>
        /// Carga en caché las etiquetas ya leídas de un alistamiento existente
        /// </summary>
        private async Task CargarEtiquetasLeidasEnCache()
        {
            try
            {
                var etiquetas = await _alistamientoEtiquetaService.ObtenerEtiquetasLeidas(_idAlistamiento);

                // Limpiar cachés antes de cargar
                _conteoPorItem.Clear();
                etiquetasLeidas.Clear();

                // Reconstruir el conteo por item y la lista de etiquetas leídas
                foreach (var etiqueta in etiquetas)
                {
                    if (!string.IsNullOrWhiteSpace(etiqueta.ETIQUETA))
                    {
                        etiquetasLeidas.Add(etiqueta.ETIQUETA);
                    }

                    if (etiqueta.ITEM.HasValue)
                    {
                        _conteoPorItem[etiqueta.ITEM.Value] = _conteoPorItem.TryGetValue(etiqueta.ITEM.Value, out var count) ? count + 1 : 1;
                    }
                }

                // Log para debug (opcional)
                System.Diagnostics.Debug.WriteLine($"Cargadas {etiquetasLeidas.Count} etiquetas en caché para alistamiento {_idAlistamiento}");
                System.Diagnostics.Debug.WriteLine($"Conteo por item: {string.Join(", ", _conteoPorItem.Select(kvp => $"{kvp.Key}:{kvp.Value}"))}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar etiquetas en caché: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Recalcula las cantidades alistadas en el DataGridView basándose en las etiquetas ya leídas
        /// </summary>
        private async Task RecalcularCantidadesAlistadas()
        {
            try
            {
                var lista = (List<InformacionCamionDTO>)dgvMain.DataSource;
                if (lista == null) return;

                // Obtener todas las etiquetas leídas para este alistamiento
                var etiquetasLeidasBD = await _alistamientoEtiquetaService.ObtenerEtiquetasLeidas(_idAlistamiento);

                // Crear diccionarios para acumular por item según tipo de producto
                var pesosPorItem = new Dictionary<int, float>(); // Para ETIQUETA_LINER (kilos)
                var unidadesPorItem = new Dictionary<int, int>(); // Para ETIQUETA (pacas/unidades)
                var metrosPorItem = new Dictionary<int, float>(); // Para ETIQUETA_ROLLO (metros)

                // Procesar cada etiqueta leída para calcular según su tipo
                foreach (var etiquetaLeida in etiquetasLeidasBD)
                {
                    if (etiquetaLeida.ITEM.HasValue && !string.IsNullOrWhiteSpace(etiquetaLeida.ETIQUETA))
                    {
                        try
                        {
                            int item = etiquetaLeida.ITEM.Value;
                            string codigoEtiqueta = etiquetaLeida.ETIQUETA;

                            // 1. Buscar en ETIQUETA_ROLLO (METROS)
                            var etiquetaRollo = await _etiquetaRolloService.ObtenerEtiquetaRolloPorCodigoAsync(codigoEtiqueta);
                            if (etiquetaRollo != null)
                            {
                                float metros = etiquetaRollo.Metros ?? 0;
                                metrosPorItem[item] = metrosPorItem.TryGetValue(item, out var metrosActual) ? metrosActual + metros : metros;
                                continue;
                            }

                            // 2. Buscar en ETIQUETA_LINER (KILOS/PESO)
                            var etiquetaLiner = await _etiquetaLinerService.ObtenerEtiquetaLinerPorCodigoAsync(codigoEtiqueta);
                            if (etiquetaLiner != null)
                            {
                                float peso = (float)etiquetaLiner.PESO_NETO;
                                pesosPorItem[item] = pesosPorItem.TryGetValue(item, out var pesoActual) ? pesoActual + peso : peso;
                                continue;
                            }

                            // 3. Buscar en ETIQUETA (UNIDADES/PACAS)
                            var etiquetaNormal = await _etiquetaService.ObtenerEtiquetaPorCodigoAsync(codigoEtiqueta);
                            if (etiquetaNormal != null)
                            {
                                unidadesPorItem[item] = unidadesPorItem.TryGetValue(item, out var unidadesActual) ? unidadesActual + 1 : 1;
                                continue;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error procesando etiqueta {etiquetaLeida.ETIQUETA}: {ex.Message}");
                        }
                    }
                }

                // Actualizar el DataGridView con las cantidades recalculadas
                foreach (var registro in lista)
                {
                    // Actualizar PACAS/UNIDADES (ETIQUETA)
                    if (unidadesPorItem.TryGetValue(registro.Item, out var cantidadAlistada))
                    {
                        registro.PacasAlistadas = cantidadAlistada;
                        registro.PacasRestantes = registro.PacasEsperadas - registro.PacasAlistadas;
                    }

                    // Actualizar KILOS (ETIQUETA_LINER)
                    if (pesosPorItem.TryGetValue(registro.Item, out var pesoAlistado))
                    {
                        registro.KilosAlistados = pesoAlistado;
                        registro.KilosRestantes = registro.KilosEsperados - registro.KilosAlistados;
                    }

                    // Actualizar METROS (ETIQUETA_ROLLO)
                    if (metrosPorItem.TryGetValue(registro.Item, out var metrosAlistados))
                    {
                        registro.MetrosAlistados = metrosAlistados;
                        registro.MetrosRestantes = registro.MetrosEsperados - registro.MetrosAlistados;
                    }
                }

                // Refrescar el grid para mostrar los cambios
                dgvMain.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al recalcular cantidades alistadas: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Obtiene el peso de una etiqueta específica
        /// </summary>
        /// <param name="codigoEtiqueta">Código de la etiqueta</param>
        /// <returns>Peso de la etiqueta</returns>
        private async Task<float> ObtenerPesoDeEtiqueta(string codigoEtiqueta)
        {
            // Buscar primero en Liner
            var etiquetaLiner = await _etiquetaLinerService.ObtenerEtiquetaLinerPorCodigoAsync(codigoEtiqueta);
            if (etiquetaLiner != null)
            {
                return (float)etiquetaLiner.PESO_NETO;
            }

            // Si no existe en Liner, buscar en Etiqueta normal
            var etiquetaNormal = await _etiquetaService.ObtenerEtiquetaPorCodigoAsync(codigoEtiqueta);
            if (etiquetaNormal != null)
            {
                return etiquetaNormal.PESO;
            }

            return 0f; // Si no se encuentra, retornar 0
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            ActualizarTimer();
        }

        private void ActualizarTimer()
        {
            var transcurrido = DateTime.Now - _fechaInicio;
            lblTimer.Text = transcurrido.ToString(@"hh\:mm\:ss");
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var placa = CAMION_ACTUAL?.PLACAS ?? string.Empty;

            // Create a properly initialized DataGridView with the current main data
            var dgvNew = new DataGridView();
            var itemsData = await _alistamientoService.ObtenerItemsPorAlistarCamion(_idCamion);
            dgvNew.DataSource = itemsData;

            var consultaForm = new LECTURA_DE_BANDA.CONSULTA_ITEMS_ETIQUETAS(ITEMS_PENDIENTES, placa, _dataGridViewExporter.ToDataTable<CamionItemsDto>(itemsData));
            consultaForm.ShowDialog(this);
        }

        private void ALISTAMIENTO_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si el estado no contiene 'ALISTADO', mostrar el cuadro de observaciones
            if (!_estadoAlistamiento.Contains("ALISTADO"))
            {
                using (var obsForm = new OBSERVACIONES(_idAlistamiento, "Indique el motivo de cierre del alistamiento"))
                {
                    var result = obsForm.ShowDialog(this);
                    if (result == DialogResult.OK && obsForm.AlistamientoAnulado)
                    {
                        // Se aceptó la observación y se anuló el alistamiento
                        _estadoAlistamiento = "ALISTADO_INCOMPLETO";
                        e.Cancel = false;
                    }
                    else
                    {
                        // Canceló o no completó la observación
                        e.Cancel = true;
                    }
                }
            }
            else
            {
                e.Cancel = false;
            }
        }

        private async Task CargarEtiquetasLeidas()
        {
            var etiquetas = await _alistamientoEtiquetaService.ObtenerEtiquetasLeidas(_idAlistamiento);

            // Inicializar BindingList una sola vez y enlazar al DGV
            _leidasBinding = new BindingList<DTOs.EtiquetaLeidaDTO>(etiquetas.ToList());
            dgvLeidos.DataSource = _leidasBinding;
            if (dgvLeidos.Columns.Count > 0)
            {
                dgvLeidos.Columns["ETIQUETA"].HeaderText = "ETIQUETA";
                dgvLeidos.Columns["ITEM"].HeaderText = "ITEM";
                dgvLeidos.Columns["DESCRIPCION"].HeaderText = "DESCRIPCIÓN";
                dgvLeidos.Columns["AREA"].HeaderText = "ÁREA";
                dgvLeidos.Columns["FECHA"].HeaderText = "FECHA";
            }

            // Reconstruir el conteo por item con lo ya leído desde DB (solo al cargar)
            _conteoPorItem.Clear();
            etiquetasLeidas.Clear();
            foreach (var et in etiquetas)
            {
                if (!string.IsNullOrWhiteSpace(et.ETIQUETA)) etiquetasLeidas.Add(et.ETIQUETA);
                if (et.ITEM.HasValue)
                {
                    _conteoPorItem[et.ITEM.Value] = _conteoPorItem.TryGetValue(et.ITEM.Value, out var c) ? c + 1 : 1;
                }
            }
        }

        private async Task cargarDgvMain()
        {
            // Traes la planificación
            IEnumerable<PlanificacionCamionDTO> planificado =
                await _alistamientoEtiquetaService.ObtenerPlanificacionCamionAsync(this._idCamion);

            // Mapeas a InformacionCamionDTO
            var informacion = planificado.Select(p => new InformacionCamionDTO
            {
                Item = p.Item,
                Descripcion = p.Descripcion,
                PacasEsperadas = p.PacasEsperadas,
                KilosEsperados = p.KilosEsperados,
                MetrosEsperados = p.MetrosEsperados,

                // Inicialmente en 0 (se recalculan después)
                PacasAlistadas = 0,
                KilosAlistados = 0,
                MetrosAlistados = 0,
                PacasRestantes = p.PacasEsperadas,
                KilosRestantes = p.KilosEsperados,
                MetrosRestantes = p.MetrosEsperados,
                CantidadPlanificada = p.CantidadPlanificada
            }).ToList();

            // Asignas al DataGridView
            this.dgvMain.AutoGenerateColumns = true;
            this.dgvMain.DataSource = informacion;

            this.dgvMain.Columns["Item"].DisplayIndex = 0;
            this.dgvMain.Columns["Descripcion"].DisplayIndex = 1;
            this.dgvMain.Columns["PacasEsperadas"].DisplayIndex = 2;
            this.dgvMain.Columns["PacasAlistadas"].DisplayIndex = 3;
            this.dgvMain.Columns["PacasRestantes"].DisplayIndex = 4;
            this.dgvMain.Columns["KilosEsperados"].DisplayIndex = 5;
            this.dgvMain.Columns["KilosAlistados"].DisplayIndex = 6;
            this.dgvMain.Columns["KilosRestantes"].DisplayIndex = 7;
            this.dgvMain.Columns["MetrosEsperados"].DisplayIndex = 8;
            this.dgvMain.Columns["MetrosAlistados"].DisplayIndex = 9;
            this.dgvMain.Columns["MetrosRestantes"].DisplayIndex = 10;
            this.dgvMain.Columns["CantidadPlanificada"].DisplayIndex = 11;

            // Para las columnas "Restantes"
            this.dgvMain.Columns["PacasRestantes"].DefaultCellStyle.BackColor = Color.Green;
            this.dgvMain.Columns["PacasRestantes"].DefaultCellStyle.ForeColor = Color.White;

            this.dgvMain.Columns["KilosRestantes"].DefaultCellStyle.BackColor = Color.Green;
            this.dgvMain.Columns["KilosRestantes"].DefaultCellStyle.ForeColor = Color.White;

            this.dgvMain.Columns["MetrosRestantes"].DefaultCellStyle.BackColor = Color.Green;
            this.dgvMain.Columns["MetrosRestantes"].DefaultCellStyle.ForeColor = Color.White;


        }

        public async Task actualizarDgvMain(string etiqueta)
        {
            // 1. Buscar primero en Liner
            var etiquetaLiner = await _etiquetaLinerService.ObtenerEtiquetaLinerPorCodigoAsync(etiqueta);

            // 2. Si no existe en Liner, buscar en Etiqueta normal
            Etiqueta etiquetaNormal = null;
            if (etiquetaLiner == null)
            {
                etiquetaNormal = await _etiquetaService.ObtenerEtiquetaPorCodigoAsync(etiqueta);
            }

            // 3. Si no existe en ninguna de las anteriores, buscar en EtiquetaRollo
            EtiquetaRollo etiquetaRollo = null;
            if (etiquetaLiner == null && etiquetaNormal == null)
            {
                etiquetaRollo = await _etiquetaRolloService.ObtenerEtiquetaRolloPorCodigoAsync(etiqueta);
            }

            // 4. Determinar los datos base de la etiqueta
            int item;
            float cantidad = 0;
            float peso = 0;
            int metros = 0;

            if (etiquetaLiner != null)
            {
                // KILOS (ETIQUETA_LINER)
                item = etiquetaLiner.ITEM;
                cantidad = etiquetaLiner.CANTIDAD;
                peso = (float)etiquetaLiner.PESO_NETO;
            }
            else if (etiquetaNormal != null)
            {
                // PACAS (ETIQUETA)
                item = etiquetaNormal.COD_ITEM;
                cantidad = etiquetaNormal.CANTIDAD;
                peso = etiquetaNormal.PESO;
            }
            else if (etiquetaRollo != null)
            {
                // METROS (ETIQUETA_ROLLO)
                item = etiquetaRollo.Item; // ✅ Propiedad PascalCase
                metros = etiquetaRollo.Metros ?? 0; // ✅ Propiedad PascalCase
            }
            else
            {
                MessageBox.Show($"Etiqueta {etiqueta} no encontrada.");
                return;
            }

            // 5. Actualizar el caché (el DataSource del DataGridView)
            var lista = (List<InformacionCamionDTO>)this.dgvMain.DataSource;
            var registro = lista.FirstOrDefault(r => r.Item == item);

            if (registro != null)
            {
                // Sumar cantidades alistadas según el tipo de producto
                if (etiquetaLiner != null)
                {
                    // KILOS
                    registro.KilosAlistados += peso;
                    registro.KilosRestantes = registro.KilosEsperados - registro.KilosAlistados;
                }
                else if (etiquetaNormal != null)
                {
                    // PACAS
                    registro.PacasAlistadas += 1; // cada etiqueta cuenta como 1 paca
                    registro.PacasRestantes = registro.PacasEsperadas - registro.PacasAlistadas;
                }
                else if (etiquetaRollo != null)
                {
                    // METROS
                    registro.MetrosAlistados += metros;
                    registro.MetrosRestantes = registro.MetrosEsperados - registro.MetrosAlistados;
                }

                // Refrescar el grid
                this.dgvMain.Refresh();
            }
            else
            {
                MessageBox.Show($"El item {item} de la etiqueta no está en la planificación.");
                lstErrores.Items.Insert(0, $"N.P: {etiqueta}");
            }
        }

        private async void txtEtiqueta_TextChanged(object sender, EventArgs e)
        {
            string etiqueta = txtEtiqueta.Text.Trim().Replace("\r", "").Replace("\n", "").ToUpper();

            // Procesar solo si la longitud limpia es 10
            if (etiqueta.Length == 10)
            {
                // Detección de duplicados en memoria (turno vigente)
                if (etiquetasLeidas.Contains(etiqueta))
                {
                    // SONIDO DE ERROR FUERTE
                    ReproducirSonidoError();
                    lstErrores.Items.Insert(0, $"DUP: {etiqueta}");
                    txtEtiqueta.Clear();
                    return;
                }

                var resultado = await _etiquetaService.ValidarEtiquetaEnKardexAsync(etiqueta);
                if (!resultado.EsValida || !resultado.Existe)
                {
                    // SONIDO DE ERROR FUERTE
                    ReproducirSonidoError();
                    lstErrores.Items.Insert(0, resultado.Mensaje);
                    txtEtiqueta.Clear();
                    return;
                }

                // Validar si el item de la etiqueta está en los ítems del camión
                int itemEtiqueta = resultado.TipoEtiqueta == "ETIQUETA"
                    ? resultado.Etiqueta.COD_ITEM
                    : (resultado.TipoEtiqueta == "ETIQUETA_LINER"
                        ? resultado.EtiquetaLiner.ITEM
                        : resultado.EtiquetaRollo.Item); // ✅ Propiedad PascalCase

                if (!ITEMS_PENDIENTES.Contains(itemEtiqueta.ToString()))
                {
                    // SONIDO DE ERROR FUERTE
                    ReproducirSonidoError();
                    lstErrores.Items.Insert(0, $"N.P: {etiqueta}");
                    txtEtiqueta.Clear();
                    return;
                }

                // Obtener la fila del pedido
                var lista = (List<InformacionCamionDTO>)dgvMain.DataSource;
                var registroItem = lista?.FirstOrDefault(r => r.Item == itemEtiqueta);

                if (registroItem == null)
                {
                    // SONIDO DE ERROR FUERTE
                    ReproducirSonidoError();
                    lstErrores.Items.Insert(0, $"No se encontró el ítem de la etiqueta en el pedido: {etiqueta}");
                    txtEtiqueta.Clear();
                    return;
                }

                // ✅ VALIDACIÓN CRÍTICA: Verificar que haya cantidad restante según el tipo de producto
                if (resultado.TipoEtiqueta == "ETIQUETA")
                {
                    // PACAS - Validar que haya pacas restantes
                    if (registroItem.PacasRestantes <= 0)
                    {
                        ReproducirSonidoError();
                        lstErrores.Items.Insert(0, $"No hay pacas restantes para el ítem {itemEtiqueta}: {etiqueta}");
                        txtEtiqueta.Clear();
                        return;
                    }
                }
                else if (resultado.TipoEtiqueta == "ETIQUETA_LINER")
                {
                    // KILOS - Validar que haya kilos restantes
                    float pesoEtiqueta = (float)resultado.EtiquetaLiner.PESO_NETO;
                    if (registroItem.KilosRestantes <= 0 || registroItem.KilosRestantes - pesoEtiqueta < -0.01) // Tolerancia mínima
                    {
                        ReproducirSonidoError();
                        lstErrores.Items.Insert(0, $"No hay kilos restantes para el ítem {itemEtiqueta}: {etiqueta}");
                        txtEtiqueta.Clear();
                        return;
                    }
                }
                else if (resultado.TipoEtiqueta == "ETIQUETA_ROLLO")
                {
                    // METROS - Validar que haya metros restantes
                    int metrosEtiqueta = resultado.EtiquetaRollo.Metros ?? 0;
                    if (registroItem.MetrosRestantes <= 0 || registroItem.MetrosRestantes - metrosEtiqueta < 0)
                    {
                        ReproducirSonidoError();
                        lstErrores.Items.Insert(0, $"No hay metros restantes para el ítem {itemEtiqueta}: {etiqueta}");
                        txtEtiqueta.Clear();
                        return;
                    }
                }

                // Validar si hay cantidad restante según el tipo de producto (LEGACY - Puede eliminarse si la validación anterior es suficiente)
                var filaPedido = dgvMain.Rows
                    .Cast<DataGridViewRow>()
                    .FirstOrDefault(r => r.Cells["Item"].Value != null && r.Cells["Item"].Value.ToString() == itemEtiqueta.ToString());

                if (filaPedido != null)
                {
                    // Solo validar duplicados para PACAS (ETIQUETA), no para Kilos ni Metros
                    if (resultado.TipoEtiqueta == "ETIQUETA")
                    {
                        int plan = 0;
                        int.TryParse(filaPedido.Cells["PacasEsperadas"].Value?.ToString(), out plan);
                        int yaLeidas = _conteoPorItem.TryGetValue(itemEtiqueta, out var c) ? c : 0;
                        if (yaLeidas >= plan)
                        {
                            // SONIDO DE ERROR FUERTE
                            ReproducirSonidoError();
                            lstErrores.Items.Insert(0, $"No hay pacas restantes para el ítem: {etiqueta}");
                            txtEtiqueta.Clear();
                            return;
                        }
                    }
                }

                // Insertar en ALISTAMIENTO_ETIQUETA
                string areaFinal = "ALISTAMIENTO-" + CAMION_ACTUAL.PLACAS; // Area dinámica según el camión

                KardexBodega ubicacionBodega = await _kardexService.ObtenerKardexDeEtiqueta(etiqueta);

                if (ubicacionBodega == null)
                {
                    // SONIDO DE ERROR FUERTE
                    ReproducirSonidoError();
                    lstErrores.Items.Insert(0, $"Error al obtener la ubicación en bodega para la etiqueta: {etiqueta}");
                    txtEtiqueta.Clear();
                    return;
                }

                await _alistamientoEtiquetaService.InsertarAlistamientoEtiquetaAsync(
                    _idAlistamiento, etiqueta, DateTime.Now, "ACTIVA",
                    ubicacionBodega.Area, areaFinal, ubicacionBodega.IdBodega, 1, UserLoginCache.IdUser);

                // ACTUALIZAR KARDEX_BODEGA para reflejar el movimiento
                ubicacionBodega.Area = areaFinal;
                ubicacionBodega.IdBodega = 1; // Bodega PRINCIPAL donde se está alistando todo.
                _kardexService.UpdateAsync(ubicacionBodega);

                // ✅ GUARDAR EN CACHÉ LOCAL - TODAS LAS ETIQUETAS (PACAS, KILOS, METROS)
                etiquetasLeidas.Add(etiqueta);

                // Solo incrementar el contador para PACAS (para validar límite de cantidad)
                if (resultado.TipoEtiqueta == "ETIQUETA")
                {
                    _conteoPorItem[itemEtiqueta] = _conteoPorItem.TryGetValue(itemEtiqueta, out var cnt) ? cnt + 1 : 1;
                }

                // Actualizar resumen principal
                await this.actualizarDgvMain(etiqueta);

                // SONIDO DE ÉXITO
                ReproducirSonidoExito();

                // Mostrar mensaje flotante sin bloquear la UI (autocierre rápido)
                string descripcionItem = filaPedido?.Cells["Descripcion"].Value?.ToString() ?? "";
                string tipoProducto = resultado.TipoEtiqueta == "ETIQUETA" ? "PACA"
                    : (resultado.TipoEtiqueta == "ETIQUETA_LINER" ? "KILOS" : "METROS");

                var msg = new MensajeFlotanteForm($"ETIQUETA LEIDA: {etiqueta}\nTIPO: {tipoProducto}\nITEM: {itemEtiqueta}\nDESCRIPCIÓN: {descripcionItem}");
                msg.Show();
                var closeTimer = new Timer { Interval = 2000 };
                closeTimer.Tick += (s2, e2) =>
                {
                    try
                    {
                        closeTimer.Stop();
                        closeTimer.Dispose();
                        if (!msg.IsDisposed)
                        {
                            msg.Close();
                            msg.Dispose();
                        }
                    }
                    catch { }
                };
                closeTimer.Start();

                // Actualizar dgvLeidos localmente sin ir a la base de datos
                _leidasBinding.Add(new DTOs.EtiquetaLeidaDTO
                {
                    ETIQUETA = etiqueta,
                    ITEM = itemEtiqueta,
                    DESCRIPCION = descripcionItem,
                    AREA = areaFinal,
                    FECHA = DateTime.Now
                });

                txtEtiqueta.Clear();
            }
        }

        /// <summary>
        /// Reproduce un sonido de error fuerte para alertar al operario
        /// </summary>
        private void ReproducirSonidoError()
        {
            try
            {
                // Múltiples beeps cortos y fuertes para mayor alerta
                Task.Run(() =>
                {
                    for (int i = 0; i < 3; i++)
                    {
                        Console.Beep(800, 200); // Frecuencia alta (800 Hz) por 200ms
                        Thread.Sleep(100);      // Pausa corta entre beeps
                    }
                });

                // También reproducir el sonido de exclamación del sistema
                SystemSounds.Exclamation.Play();
            }
            catch (Exception ex)
            {
                // En caso de error al reproducir sonido, registrar en debug pero no interrumpir el flujo
                System.Diagnostics.Debug.WriteLine($"Error al reproducir sonido de error: {ex.Message}");
            }
        }

        /// <summary>
        /// Reproduce un sonido de éxito cuando la etiqueta se lee correctamente
        /// </summary>
        private void ReproducirSonidoExito()
        {
            try
            {
                // Un beep suave de confirmación
                Task.Run(() =>
                {
                    Console.Beep(1000, 150); // Frecuencia más alta (1000 Hz) por 150ms
                });
            }
            catch (Exception ex)
            {
                // En caso de error al reproducir sonido, registrar en debug pero no interrumpir el flujo
                System.Diagnostics.Debug.WriteLine($"Error al reproducir sonido de éxito: {ex.Message}");
            }
        }

        private void btnPausa_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("¿Desea PONER EN PAUSA el Alistamiento?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (res.Equals(true))
            {
                _alistamientoService.ActualizarAlistamiento(_idAlistamiento, "ALISTADO_INCOMPLETO", "Alistamiento pausado por el usuario", DateTime.Now);
                MessageBox.Show("Alistamiento PAUSADO.", "Pausa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            this.Close();

        }
    }

}