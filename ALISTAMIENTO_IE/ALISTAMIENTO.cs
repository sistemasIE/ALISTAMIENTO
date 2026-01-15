using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Forms;
using ALISTAMIENTO_IE.Interfaces;
using ALISTAMIENTO_IE.Models;
using Common.cache;
using LECTURA_DE_BANDA;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using System.ComponentModel; // Added for BindingList
using System.Data;
using System.Media;
using Timer = System.Windows.Forms.Timer;

namespace ALISTAMIENTO_IE
{
    public partial class ALISTAMIENTO : Form
    {
        // --- PROPIEDADES INYECTADAS (readonly) ---
        private readonly IEtiquetaLinerService _etiquetaLinerService;
        private readonly IEtiquetaRolloService _etiquetaRolloService;
        private readonly IEtiquetaService _etiquetaService;
        private readonly IAlistamientoEtiquetaService _alistamientoEtiquetaService;
        private readonly IAlistamientoService _alistamientoService;
        private readonly IAuthorizationService _authorizationService;
        private readonly ICamionService _camionService;
        private readonly IDetalleCamionXDiaService _detalleCamionXDiaService;
        private readonly IEliminacionAlistamientoEtiquetaService _elimService;
        private readonly IKardexService _kardexService;
        private readonly IServiceScopeFactory _scopeFactory; // Reemplaza IServiceProvider

        // --- PROPIEDADES DE ESTADO Y CONTEXTO ---
        private readonly int _idCamion;
        private readonly string _estadoAlistamientoInicial;

        // Propiedades que aceptan nulos o requieren inicialización en el ctor
        private Timer? _timer;
        private Camion? CAMION_ACTUAL; // Puede ser null si el servicio no encuentra el camión
        private IEnumerable<EtiquetaLeidaDTO>? _etiquetasLeidas;
        public Alistamiento? _alistamiento; // Objeto de modelo, si puede ser null

        // Propiedades con inicialización directa
        private readonly DataGridViewExporter _dataGridViewExporter = new DataGridViewExporter();
        private BindingList<DTOs.EtiquetaLeidaDTO> _leidasBinding = new();
        private readonly Dictionary<int, int> _conteoPorItem = new();
        private List<string> etiquetasLeidas = new();
        private List<string> ITEMS_PENDIENTES = new();

        // Valores por defecto
        private string _estadoAlistamiento = "EN_PROCESO";
        private int _idAlistamiento;
        private DateTime _fechaInicio;

        private bool cerradoDesdeBoton = false;

        // ------------------------------------------------------------------
        // CONSTRUCTORES
        // ------------------------------------------------------------------

        // Constructor de sobrecarga: Delega al principal con estado "EN_PROCESO"
        public ALISTAMIENTO(
            int idCamion,
            IAlistamientoService alistamientoService,
            IAlistamientoEtiquetaService alistamientoEtiquetaService,
            IServiceScopeFactory scopeFactory,
            IAuthorizationService authorizationService,
            ICamionService camionService,
            IDetalleCamionXDiaService detalleCamionXDiaService,
            IEtiquetaService etiquetaService,
            IEtiquetaLinerService etiquetaLinerService,
            IEtiquetaRolloService etiquetaRolloService,
            IEliminacionAlistamientoEtiquetaService elimService,
            IKardexService kardexService
        )
        : this(idCamion, "EN_PROCESO",
              alistamientoService, alistamientoEtiquetaService, scopeFactory,
              authorizationService, camionService, detalleCamionXDiaService,
              etiquetaService, etiquetaLinerService, etiquetaRolloService,
              elimService, kardexService)
        {
            // Creado automáticamente por DI.
        }

        // Constructor principal: Recibe todos los servicios y los asigna.
        public ALISTAMIENTO(
            int idCamion,
            string estado,
            // Servicios de Alistamiento
            IAlistamientoService alistamientoService,
            IAlistamientoEtiquetaService alistamientoEtiquetaService,
            // Fábrica para Forms
            IServiceScopeFactory scopeFactory,
            // Otros servicios
            IAuthorizationService authorizationService,
            ICamionService camionService,
            IDetalleCamionXDiaService detalleCamionXDiaService,
            IEtiquetaService etiquetaService,
            IEtiquetaLinerService etiquetaLinerService,
            IEtiquetaRolloService etiquetaRolloService,
            IEliminacionAlistamientoEtiquetaService elimService,
            IKardexService kardexService
        )
        {
            InitializeComponent();

            // 1. Asignación de dependencias y parámetros de contexto
            _idCamion = idCamion;
            _estadoAlistamientoInicial = estado;
            _estadoAlistamiento = estado;

            _scopeFactory = scopeFactory;

            // Asignación de Servicios
            _alistamientoEtiquetaService = alistamientoEtiquetaService;
            _alistamientoService = alistamientoService;
            _authorizationService = authorizationService;
            _camionService = camionService;
            _detalleCamionXDiaService = detalleCamionXDiaService;
            _etiquetaService = etiquetaService;
            _etiquetaLinerService = etiquetaLinerService;
            _etiquetaRolloService = etiquetaRolloService;
            _elimService = elimService;
            _kardexService = kardexService;

            // 2. Inicialización de clases de UI o de Contexto
            _timer = new Timer(); // Inicialización de Timer

            // 3. Uso del servicio inyectado para obtener datos iniciales
            CAMION_ACTUAL = _camionService.GetCamionByCamionXDiaId(idCamion);

            // 4. Configuración de UI
            this.dgvMain.CellDoubleClick += dgvMainDoubleClick;
            this.Load += ALISTAMIENTO_Load;
            btnBuscar.Click += btnBuscar_Click;
            this.FormClosing += ALISTAMIENTO_FormClosing;

            lblPlaca.Text = CAMION_ACTUAL != null ? $"PLACAS: " + CAMION_ACTUAL.PLACAS.ToString() : "Desconocida";
            this.Icon = ALISTAMIENTO_IE.Properties.Resources.Icono;
        }

        private bool VerificarAlistamientoCompleto()
        {
            var lista = (List<InformacionCamionDTO>)dgvMain.DataSource;
            if (lista == null || !lista.Any())
                return true;

            foreach (var item in lista)
            {
                // Validar según qué campo tiene valores esperados > 0
                if (item.PacasEsperadas > 0 && item.PacasRestantes > 0)
                    return false; // Faltan pacas

                if (item.KilosEsperados > 0 && item.KilosRestantes > 0.01) // Tolerancia 10g
                    return false; // Faltan kilos

                if (item.MetrosEsperados > 0 && item.MetrosRestantes > 0)
                    return false; // Faltan metros
            }

            return true; // Todo completado
        }


        private void BtnTerminar_Click(object sender, EventArgs e)
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
                    cerradoDesdeBoton = true;
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
                        // *** 1. Crear un nuevo ámbito para el Formulario ***
                        using (var scope = _scopeFactory.CreateScope())
                        {
                            // *** 2. Usar ActivatorUtilities para instanciar el Form. ***
                            // ActivatorUtilities se encarga de inyectar IAlistamientoService.
                            using (var obsForm = ActivatorUtilities.CreateInstance<OBSERVACIONES>(
                                scope.ServiceProvider,
                                _idAlistamiento,
                                "Alistamiento INCOMPLETO", // Se cierra con alistamiento incompleto
                                "Indique el motivo del alistamiento incompleto" // Parámetros de contexto
                            ))
                            {
                                var obsResult = obsForm.ShowDialog(this);
                                if (obsResult == DialogResult.OK && obsForm.AlistamientoIncompleto)
                                {
                                    // El estado ya fue actualizado en el form de observaciones
                                    _estadoAlistamiento = "ALISTADO";
                                    MessageBox.Show("Alistamiento cerrado como incompleto.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    cerradoDesdeBoton = true;
                                    this.Close();

                                }
                            } // obsForm se desecha
                        } // scope se desecha, liberando todos los servicios scoped que usó el form
                    }
                }
        }
        private async void ALISTAMIENTO_Load(object sender, EventArgs e)
        {
            try
            {
                _fechaInicio = DateTime.Now;

                if (_estadoAlistamientoInicial == AlistamientoEstado.ALISTADO_INCOMPLETO.ToString())
                {
                    // CASO ESPECIAL: Continuar un alistamiento incompleto
                    Alistamiento alistamientoIncompleto = _alistamientoService.ObtenerAlistamientoPorCodCamionYEstado(_idCamion, AlistamientoEstado.ALISTADO_INCOMPLETO.ToString());

                    if (alistamientoIncompleto == null)
                    {
                        MessageBox.Show("Error: No se encontró un alistamiento en estado 'ALISTADO_INCOMPLETO' para este camión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }

                    _alistamiento = alistamientoIncompleto;

                    _idAlistamiento = alistamientoIncompleto.IdAlistamiento;
                    _fechaInicio = alistamientoIncompleto.FechaInicio;

                    // IMPORTANTE: Cambiar el estado a EN_PROCESO para poder continuar alistando
                    _alistamientoService.ActualizarAlistamiento(_idAlistamiento, "EN_PROCESO", "Continuando alistamiento incompleto", null);
                    _estadoAlistamiento = "EN_PROCESO";

                    await CargarEtiquetasLeidasEnCache();

                    // Recalcular cantidades alistadas basándose en las etiquetas ya leídas
                    await RecalcularCantidadesAlistadas();

                }
                else if (_estadoAlistamientoInicial == "SIN_ALISTAR")
                {
                    // Crear un nuevo alistamiento
                    _idAlistamiento = _alistamientoService.InsertarAlistamiento(_idCamion, UserLoginCache.IdUser);
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
                    await CargarEtiquetasLeidasEnCache();


                    // Recalcular cantidades alistadas basándose en las etiquetas ya leídas
                    await RecalcularCantidadesAlistadas();

                }

                // Inicializar timer
                _timer = new Timer { Interval = 10000 }; // 10 segundos
                _timer.Tick += Timer_Tick;
                _timer.Start();
                ActualizarTimer();

                // Construir (Kilos Esperados, Pacas Esperadas y demás)
                await cargarDgvMain();


                // Obtener los ítems pendientes del camión
                IEnumerable<CamionItemsDto> items = await _alistamientoService.ObtenerItemsPorAlistarCamion(_idCamion);
                ITEMS_PENDIENTES = items.Select(i => i.Item.ToString()).ToList();

                btnPausa.Cursor = Cursors.Hand;
                btnBuscar.Cursor = Cursors.Hand;
                btnTerminar.Cursor = Cursors.Hand;
                txtEtiqueta.Cursor = Cursors.Hand;
                txtEtiqueta.Enabled = true;

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
                _etiquetasLeidas = await _alistamientoEtiquetaService.ObtenerEtiquetasLeidas(_alistamiento.IdAlistamiento);

                // Inicializar BindingList una sola vez y enlazar al DGV
                _leidasBinding = new BindingList<DTOs.EtiquetaLeidaDTO>(_etiquetasLeidas.ToList());
                dgvLeidos.DataSource = _leidasBinding;


                // Limpiar cachés antes de cargar
                _conteoPorItem.Clear();
                etiquetasLeidas.Clear();

                // Reconstruir el conteo por item y la lista de etiquetas leídas
                foreach (EtiquetaLeidaDTO etiqueta in _etiquetasLeidas)
                {
                    if (!string.IsNullOrWhiteSpace(etiqueta.ETIQUETA))
                    {
                        etiquetasLeidas.Add(etiqueta.ETIQUETA);

                        if (etiqueta.ITEM.HasValue)
                        {
                            _conteoPorItem[etiqueta.ITEM.Value] = _conteoPorItem.TryGetValue(etiqueta.ITEM.Value, out var count) ? count + 1 : 1;
                        }

                    }

                }

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

                // Crear diccionarios para acumular por item según tipo de producto
                var pesosPorItem = new Dictionary<int, float>(); // Para ETIQUETA_LINER (kilos)
                var unidadesPorItem = new Dictionary<int, int>(); // Para ETIQUETA (pacas/unidades)
                var metrosPorItem = new Dictionary<int, float>(); // Para ETIQUETA_ROLLO (metros)

                // ✅ OPTIMIZACIÓN: Usar directamente _etiquetasLeidas que ya está cargada en memoria
                // Procesar cada etiqueta leída para calcular según su tipo (DESDE)
                foreach (var etiquetaLeida in _etiquetasLeidas)
                {
                    if (!etiquetaLeida.ITEM.HasValue) continue;

                    int item = etiquetaLeida.ITEM.Value;
                    float valor = etiquetaLeida.VALOR;

                    // Clasificar según el tipo de producto (DESDE)
                    switch (etiquetaLeida.DESDE)
                    {
                        case TipoProductoEnum.SACOS: // ETIQUETA (UNIDADES/PACAS)
                            unidadesPorItem[item] = unidadesPorItem.TryGetValue(item, out var unidadesActual)
                                ? unidadesActual + 1
                                : 1;
                            break;

                        case TipoProductoEnum.LINER: // ETIQUETA_LINER (KILOS/PESO)
                            pesosPorItem[item] = pesosPorItem.TryGetValue(item, out var pesoActual)
                                ? pesoActual + valor
                                : valor;
                            break;

                        case TipoProductoEnum.ROLLO: // ETIQUETA_ROLLO (METROS)
                            metrosPorItem[item] = metrosPorItem.TryGetValue(item, out var metrosActual)
                                ? metrosActual + valor
                                : valor;
                            break;
                    }
                }

                // Actualizar el DataGridView con las cantidades recalculadas
                foreach (var registro in lista)
                {
                    // Actualizar PACAS/UNIDADES (ETIQUETA/SACOS)
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
            var itemsData = await _alistamientoService.ObtenerItemsPorAlistarCamion(_idCamion);

            // Convertir datos a DataTable ANTES de pasarlos
            var infoDataTable = _dataGridViewExporter.ToDataTable<CamionItemsDto>(itemsData);

            using (var scope = _scopeFactory.CreateScope())
            {
                var consultaForm = ActivatorUtilities.CreateInstance<CONSULTA_ITEMS_ETIQUETAS>(
                    scope.ServiceProvider,
                    ITEMS_PENDIENTES,
                    placa,
                    infoDataTable
                );

                consultaForm.ShowDialog(this);

            }
        }

        private void ALISTAMIENTO_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Si fue cerrado desde alguno de los botones
            if (cerradoDesdeBoton) return;

            VerificarAlistamientoCompleto();

            // Si el estado no contiene 'ALISTADO', mostrar el cuadro de observaciones
            if (!_estadoAlistamiento.Contains("ALISTADO"))
            {
                using (var obsForm = new OBSERVACIONES(_idAlistamiento, "incompleto", "Indique el motivo del cierre.", _alistamientoService))
                {
                    var result = obsForm.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        // Se aceptó la observación y el alistamiento quedó incompleto
                        _estadoAlistamiento = AlistamientoEstado.ALISTADO_INCOMPLETO.ToString();
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
        private async Task cargarDgvMain()
        {
            _alistamientoService.CargarCamionDia(_idCamion, dgvMain);
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
                lstMensajes.Items.Insert(0, $"N.P: {etiqueta}");
            }


        }

        private async void txtEtiqueta_TextChanged(object sender, EventArgs e)
        {
            string etiqueta = txtEtiqueta.Text.Trim().Replace("\r", "").Replace("\n", "").ToUpper();

            // Procesar solo si la longitud limpia es 10
            if (etiqueta.Length == 10)
            {
                // ========== VALIDACIONES INICIALES ==========

                // 1. Detección de duplicados en memoria
                if (etiquetasLeidas.Contains(etiqueta))
                {
                    ReproducirSonidoError();
                    lstMensajes.Items.Insert(0, $"DUP: {etiqueta}");
                    txtEtiqueta.Clear();
                    return;
                }

                // 2. Validar en KARDEX y existencia
                var resultado = await _etiquetaService.ValidarEtiquetaEnKardexAsync(etiqueta);
                if (!resultado.EsValida || !resultado.Existe)
                {
                    ReproducirSonidoError();
                    lstMensajes.Items.Insert(0, resultado.Mensaje);
                    txtEtiqueta.Clear();
                    return;
                }

                // 3. Extraer datos según el tipo de etiqueta
                int itemEtiqueta = resultado.TipoEtiqueta == "ETIQUETA"
                    ? resultado.Etiqueta.COD_ITEM
                    : (resultado.TipoEtiqueta == "ETIQUETA_LINER"
                        ? resultado.EtiquetaLiner.ITEM
                        : resultado.EtiquetaRollo.Item);

                // 4. Validar que el ítem esté en la planificación del camión
                if (!ITEMS_PENDIENTES.Contains(itemEtiqueta.ToString()))
                {
                    ReproducirSonidoError();
                    lstMensajes.Items.Insert(0, $"!!!!!! N.P: {etiqueta} !!!!!! ");
                    txtEtiqueta.Clear();
                    return;
                }

                // 5. Obtener el registro del ítem desde el DataSource
                var lista = (List<InformacionCamionDTO>)dgvMain.DataSource;
                var registroItem = lista?.FirstOrDefault(r => r.Item == itemEtiqueta);

                if (registroItem == null)
                {
                    ReproducirSonidoError();
                    lstMensajes.Items.Insert(0, $"No se encontró el ítem de la etiqueta en el pedido: {etiqueta}");
                    txtEtiqueta.Clear();
                    return;
                }

                // ========== VALIDACIÓN CRÍTICA DE CANTIDADES RESTANTES ==========

                float valorEtiqueta = 0;
                TipoProductoEnum tipoProducto;

                if (resultado.TipoEtiqueta == "ETIQUETA")
                {
                    // PACAS - Validar que haya pacas restantes
                    if (registroItem.PacasRestantes <= 0)
                    {
                        ReproducirSonidoError();
                        lstMensajes.Items.Insert(0, $"No hay pacas restantes para el ítem {itemEtiqueta}: {etiqueta}");
                        txtEtiqueta.Clear();
                        return;
                    }
                    valorEtiqueta = resultado.Etiqueta.CANTIDAD;
                    tipoProducto = TipoProductoEnum.SACOS;
                }
                else if (resultado.TipoEtiqueta == "ETIQUETA_LINER")
                {
                    // KILOS - Validar que haya kilos restantes
                    float pesoEtiqueta = (float)resultado.EtiquetaLiner.PESO_NETO;
                    if (registroItem.KilosRestantes <= 0 || registroItem.KilosRestantes - pesoEtiqueta < -0.01)
                    {
                        ReproducirSonidoError();
                        lstMensajes.Items.Insert(0, $"No hay kilos restantes para el ítem {itemEtiqueta}: {etiqueta}");
                        txtEtiqueta.Clear();
                        return;
                    }
                    valorEtiqueta = pesoEtiqueta;
                    tipoProducto = TipoProductoEnum.LINER;
                }
                else // ETIQUETA_ROLLO
                {
                    // METROS - Validar que haya metros restantes
                    int metrosEtiqueta = resultado.EtiquetaRollo.Metros ?? 0;
                    if (registroItem.MetrosRestantes <= 0 || registroItem.MetrosRestantes - metrosEtiqueta < 0)
                    {
                        ReproducirSonidoError();
                        lstMensajes.Items.Insert(0, $"No hay metros restantes para el ítem {itemEtiqueta}: {etiqueta}");
                        txtEtiqueta.Clear();
                        return;
                    }
                    valorEtiqueta = metrosEtiqueta;
                    tipoProducto = TipoProductoEnum.ROLLO;
                }

                // ========== INSERCIÓN EN BASE DE DATOS ==========

                string areaFinal = "ALISTAMIENTO-" + CAMION_ACTUAL.PLACAS;
                KardexBodega ubicacionBodega = await _kardexService.ObtenerKardexDeEtiqueta(etiqueta);

                if (ubicacionBodega == null)
                {
                    ReproducirSonidoError();
                    lstMensajes.Items.Insert(0, $"Error al obtener la ubicación en bodega para la etiqueta: {etiqueta}");
                    txtEtiqueta.Clear();
                    return;
                }

                // Insertar en ALISTAMIENTO_ETIQUETA
                await _alistamientoEtiquetaService.InsertarAlistamientoEtiquetaAsync(
                    _idAlistamiento, etiqueta, DateTime.Now, "ACTIVA",
                    ubicacionBodega.Area, areaFinal, ubicacionBodega.IdBodega, 1, UserLoginCache.IdUser);

                // Actualizar KARDEX_BODEGA para reflejar el movimiento
                ubicacionBodega.Area = areaFinal;
                ubicacionBodega.IdBodega = 1;
                _kardexService.UpdateAsync(ubicacionBodega);

                // ========== ACTUALIZACIÓN DE CACHÉS Y UI ==========

                // Guardar en caché local
                etiquetasLeidas.Add(etiqueta);

                // Solo incrementar el contador para PACAS (para validar límite de cantidad)
                if (resultado.TipoEtiqueta == "ETIQUETA")
                {
                    _conteoPorItem[itemEtiqueta] = _conteoPorItem.TryGetValue(itemEtiqueta, out var cnt) ? cnt + 1 : 1;
                }

                // Actualizar resumen principal (dgvMain)
                await this.actualizarDgvMain(etiqueta);

                // Actualizar dgvLeidos con la nueva estructura optimizada
                _leidasBinding.Add(new DTOs.EtiquetaLeidaDTO
                {
                    ETIQUETA = etiqueta,
                    ITEM = itemEtiqueta,
                    DESCRIPCION = registroItem.Descripcion,
                    AREA = areaFinal,
                    FECHA = DateTime.Now,
                    DESDE = tipoProducto,
                    VALOR = valorEtiqueta
                });

                // ========== FEEDBACK AL USUARIO ==========

                // Sonido de éxito
                ReproducirSonidoExito();

                // Mensaje flotante con información de la etiqueta leída
                string tipoProductoTexto = tipoProducto == TipoProductoEnum.SACOS ? "PACA"
                    : (tipoProducto == TipoProductoEnum.LINER ? "KILOS" : "METROS");

                lstMensajes.Items.Insert(0, $"✓REC: {etiqueta} ITEM: {itemEtiqueta} DES: {registroItem.Descripcion.Substring(0, 10)}");

                txtEtiqueta.Clear();
            }
        }

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


        private async void dgvMainDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Evita errores por cabecera o índices fuera de rango
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            string columnaObjetivo = "Item"; // columna a controlar

            if (dgvMain.Columns[e.ColumnIndex].Name == columnaObjetivo)
            {
                // 📌 Obtienes el valor viejo ANTES de modificar
                int itemViejo = 0;
                object valorCelda = dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                if (valorCelda != null && int.TryParse(valorCelda.ToString(), out int temp))
                    itemViejo = temp;

                // 1️⃣ Confirmación inicial
                DialogResult confirmacion = MessageBox.Show(
                    $"¿Deseas modificar el valor del ITEM {itemViejo}?",
                    "Cambiar ITEM",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (confirmacion != DialogResult.Yes)
                    return;

                // 2️⃣ Solicitar nuevo valor numérico
                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    "Introduce el nuevo valor del ITEM:",
                    "Editar valor",
                    valorCelda?.ToString() ?? "0"
                );

                if (!int.TryParse(input, out int nuevoValor))
                {
                    MessageBox.Show("Valor inválido. Debe ser numérico entero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 3️⃣ Solicitar autorización con contraseña
                string password = Microsoft.VisualBasic.Interaction.InputBox(
                    "Introduce la contraseña de autorización:",
                    "Autorización requerida"
                );

                password = BCrypt.Net.BCrypt.HashPassword(password);

                int area = 21; // id del área de Alistamiento
                int idRol = 1; // Id del Rol de Administrador


                if (await _authorizationService.ValidatePasswordAsync(area, idRol, password))
                {
                    MessageBox.Show("Contraseña incorrecta. No se realizó ningún cambio.", "Denegado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 4️⃣ Aplicar el cambio si todo está correcto
                dgvMain.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = nuevoValor;

                // Llamas al servicio pasando el viejo y el nuevo
                _detalleCamionXDiaService.ActualizarItemDadoCodCamionEItem(_idCamion, itemViejo, nuevoValor);

                MessageBox.Show($"Item {itemViejo} actualizado correctamente a {nuevoValor}.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private void btnPausa_Click(object sender, EventArgs e)
        {
            var res = MessageBox.Show("¿Desea PONER EN PAUSA el Alistamiento?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

            if (res.Equals(true))
            {
                _alistamientoService.ActualizarAlistamiento(_idAlistamiento, AlistamientoEstado.ALISTADO_INCOMPLETO.ToString(), "Alistamiento pausado por el usuario", DateTime.Now);
                MessageBox.Show("Alistamiento PAUSADO.", "Pausa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            this.Close();

        }

        private void dgvLeidos_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvLeidos.SelectedCells.Count == 0)
            {
                btnEliminarEtiquetasLeidas.Visible = false;
                return;
            }

            // Verifica que todas las celdas seleccionadas sean de la columna "ETIQUETA"
            bool soloColumnaEtiqueta = dgvLeidos.SelectedCells
                .Cast<DataGridViewCell>()
                .All(c => c.OwningColumn.Name == "ETIQUETA");

            btnEliminarEtiquetasLeidas.Visible = soloColumnaEtiqueta;
        }

        private async void btnEliminarEtiquetasLeidas_Click(object sender, EventArgs e)
        {
            var etiquetasSeleccionadas = dgvLeidos.SelectedCells
                .Cast<DataGridViewCell>()
                .Select(c => c.Value?.ToString())
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct()
                .ToArray();

            if (etiquetasSeleccionadas.Length == 0)
            {
                MessageBox.Show("No hay etiquetas válidas seleccionadas.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_alistamiento == null)
            {
                MessageBox.Show("Alistamiento no inicializado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Confirmación antes de operar
            var confirm = MessageBox.Show($"¿Desea eliminar {etiquetasSeleccionadas.Length} etiqueta(s) del alistamiento?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            // 1) Pedir observaciones al usuario
            string observaciones = Interaction.InputBox("Ingrese las observaciones de la eliminación:", "Observaciones", "");

            if (string.IsNullOrWhiteSpace(observaciones))
            {
                var seguir = MessageBox.Show("No ingresó observaciones. ¿Desea continuar sin observaciones?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (seguir != DialogResult.Yes) return;
                observaciones = null;
            }

            // 2) Preparar funciones
            Func<string[], bool> validarNoVacias = etqs => etqs.All(e => e.Length >= 10);

            // Función de eliminación base
            Func<string[], Task> eliminarEtiquetas = async etqs =>
            {
                var eliminadas = await _alistamientoEtiquetaService.EliminarEtiquetasDeAlistamiento(_alistamiento.IdAlistamiento, etqs);
                if (eliminadas <= 0)
                    throw new InvalidOperationException("No se eliminaron etiquetas. Verifique estado.");
            };

            // 3) Ejecutar formulario genérico
            var form = new FormOperarEtiquetas(
                "ELIMINAR",
                etiquetasSeleccionadas,
                new List<Func<string[], bool>> { validarNoVacias },
                new List<Func<string[], Task>> { eliminarEtiquetas }
            );

            var result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                try
                {
                    // 4) Registrar auditoría por cada ID de ALISTAMIENTO_ETIQUETA
                    // Ya tenemos los IDs cargados en _leidasBinding via IDALISTAMIENTOETIQUETA
                    var idsAEliminar = _leidasBinding
                        .Where(x => etiquetasSeleccionadas.Contains(x.ETIQUETA))
                        .Select(x => x.IDALISTAMIENTOETIQUETA)
                        .Distinct()
                        .ToArray();

                    foreach (var idAliEtq in idsAEliminar)
                    {
                        await _elimService.InsertarEliminacionAsync(idAliEtq, UserLoginCache.IdUser, observaciones);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error registrando observaciones: {ex.Message}", "Auditoría", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Recargar la Interfaz
                RecargarUI();


                foreach (string etiqueta in etiquetasSeleccionadas)
                {
                    lstMensajes.Items.Insert(0, "Etiqueta eliminada: " + etiqueta);
                }
            }
        }

        private void btnVerEliminadas_Click(object sender, EventArgs e)
        {
            // Usamos _scopeFactory, que ya fue inyectado en el constructor de ALISTAMIENTO.
            using (var scope = _scopeFactory.CreateScope())
            {
                // 1. Pedimos al ServiceProvider del scope que cree la instancia.
                // ActivatorUtilities inyectará IEliminacionAlistamientoEtiquetaService automáticamente.
                var form = ActivatorUtilities.CreateInstance<EtqsEliminadasEnAlistamiento>(
                    scope.ServiceProvider,
                    _idAlistamiento // Argumento que no es una dependencia y DEBE pasarse.
                );

                // 2. Mostrar
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
        }

        private async void RecargarUI()
        {

            await CargarEtiquetasLeidasEnCache();
            await RecalcularCantidadesAlistadas();
            await cargarDgvMain();
            await RecalcularCantidadesAlistadas();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            RecargarUI();
        }
    }

}