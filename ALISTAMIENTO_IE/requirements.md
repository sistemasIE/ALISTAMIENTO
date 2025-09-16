# Requerimientos — App WinForms (.NET) + MSSQL + Dapper

> Objetivo: Gestionar alistamiento y despacho por **etiquetas** con control de turnos, vistas operativas y reportes.  
> Mantra: simple, rápido, a prueba de “ups”.

---

## 1) Autenticación & Sesión

**Login**
- Validar `USUARIO.user` y `USUARIO.password` (texto plano).
- Denegar acceso si no coincide.

**Sesión**
- Cerrar sesión automáticamente al **cerrar turno**.
- Forzar cierre de sesión al salir de la app.

---

## 2) Inicio (Pantalla principal)

**Listado de camiones activos**
- Mostrar: `Camión`, `# Remisión`, `Cantidad a despachar`.
- Fuente: camiones con estado **ACTIVO**.

**Inicio de alistamiento**
- Al seleccionar camión:
  - Crear/activar **Turno**.
  - Iniciar **cronómetro** visible (arriba-derecha).
  - Registrar `inicioTurno = GETDATE()`.

---

## 3) Gestión de Etiquetas

**Entrada**
- Campo texto `etiqueta` (escáner Honeywell/Zebra).
- Cada etiqueta debe:
  - Tener **10 caracteres**.
  - Incluir **al menos una letra**.

**Detección de duplicados**
- Si la etiqueta ya fue capturada en el turno vigente:
  - **Eliminar** la duplicada (no insertar/repetir).
  - Registrar en **errores**: `DUP: {etiqueta}`.

**Existencia**
- Validar existencia en:
  - `ETIQUETA.COD_ETIQUETA` **o**
  - `ETIQUETA_LINER.COD_ETIQUETA_LINER`.

**Kardex (ingreso automático)**
- Si **existe** en ETIQUETA/ETIQUETA_LINER **pero NO** está en `KARDEX_BODEGA`:
  - Insertar en `KARDEX_BODEGA` con:
    - `idBodega = 1`
    - `area = 'ALISTAMIENTO'`
    - `fechaEntrada = GETDATE()`
    - `tipoEntrada = 'M'`
    - `idKardexBodega = NULL`
    - `fechaSalida = NULL`

**No existencia (hard stop)**
- Si **NO existe** en ETIQUETA/ETIQUETA_LINER **y** tampoco en `KARDEX_BODEGA`:
  - Mostrar **ERROR BLOQUEANTE**: `N.E – Etiqueta: {k}`
  - No continuar flujo (separar el bulto).

- Si **NO existe** en `KARDEX_BODEGA` (independiente de tablas de origen):
  - Mostrar **ERROR BLOQUEANTE**: `N.E – Etiqueta: {k}`.

**Validación contra pedido del camión**
- Obtener **ITEM** por:
  - `ETIQUETA.COD_ITEM` **o**
  - `ETIQUETA_LINER.ITEM`.
- Si el ITEM **no está en lo solicitado por el camión**:
  - Agregar a errores: `N.P: {etiqueta}`.
- Si el ITEM está solicitado **pero** `pacASrestantes == 0`:
  - No agregar etiqueta (informar “sin cupo”).

**Alta de etiqueta válida**
- Insertar en `ALISTAMIENTO_ETIQUETA`:
  - `idCamion` (seleccionado)
  - `areaVieja` (buscar en `KARDEX_BODEGA`)
  - `areaFinal` (área seleccionada en UI)
  - `idBodegaInicial` (de Kardex)
  - `idBodegaFinal = 1`
  - `fecha = GETDATE()`
- Actualizar `KARDEX_BODEGA`:
  - `idBodega = 1`
  - `area = {AREA_ESCOGIDA_EN_LABEL}`

**Errores**
- **Persistir** todos los errores del turno en tabla de auditoría (ver “Modelo”).

**Rehacer cambios (rollback etiqueta)**
- Botón `REHACER_CAMBIOS`:
  - Eliminar la etiqueta errónea del turno.
  - Restaurar en `KARDEX_BODEGA` su **ubicación anterior**.

---

## 4) Vistas Operativas

**4.1 Ítems por PACAS / UNIDADES**
- Columnas: `ITEM`, `DESCRIPCION_ITEM`, `AREA`, `BODEGA`, `UNIDADES`.
- Filtrar por **ítems del camión activo**.
- Al completar **cantidad de pacas** (1 etiqueta = 1 paca):
  - Descontar en tiempo real las unidades restantes.
- Botón **Imprimir** (formato lista de picking).

**4.2 Tiempos por Turno (7:00 AM ↔ 7:00 AM)**
- Filtro por **FECHA** (ventana: 7 AM día -1 a 7 AM día actual).
- Columnas: `TURNO`, `FECHA`, `DIA`, `CANTIDAD`, `MÉTRICA`.

**4.3 Tiempos por Camión (7:00 AM ↔ 7:00 AM)**
- Columnas: `TURNO`, `FECHA`, `DIA`, `TIEMPO TOTAL`, `BULTOS DESPACHADOS`, `TIEMPO PROMEDIO POR BULTO`.
- Botón **Exportar a Excel**.

**4.4 Etiquetas escaneadas (en vivo)**
- Columnas: `ETIQUETA`, `ITEM`, `DESCRIPCION_ITEM`, `AREA`, `fechaRegistro`, `operacion='CAMBIO_UBICACION'`.

**4.5 Requeridos vs Progreso**
- Vista de **ITEMS requeridos** con unidades pendientes.
- Botón **Reset** para ver **estado inicial** del camión.

---

## 5) Reportes

**5.1 Métricas agrupadas por fecha**
- Columnas: `TURNO`, `MÉTRICA`, `CANTIDAD DESP.`, `CANT. CAMIONES`.

**5.2 Histórico completo (todos los turnos)**
- Orden ascendente por hora.
- Columnas: `TURNO`, `MÉTRICA`, `HORA`, `ETIQUETA`, `ITEM`, `DESCRIPCION`.

**5.3 Métricas por turno (por camión)**
- Columnas: `CAMIÓN`, `MÉTRICA`, `UNIDADES`, `ALISTADO A LAS …`.

**5.4 Detalle de un turno**
- Filtros: `TURNO` + `FECHA`.
- Orden por hora ascendente.
- Columnas: `TURNO`, `MÉTRICA`, `HORA`, `ETIQUETA`, `ITEM`, `DESCRIPCION`.

**5.5 Estadísticas**
- Por **día** y por **semana**.
- Requiere **deseleccionar fecha** antes de agrupar.

---

## 6) Cierre de Camión

**Botón “Cerrar cargue”**
- Validar que **todas las pacas solicitadas** fueron enviadas.
- Si faltan:
  - Mostrar **ERROR GRANDE**: camión quedará **INCOMPLETO**.
  - Si el usuario **acepta**, guardar con `OBSERVACIONES`:
    - Formato: `ITEM: n, CANTIDAD FALTANTE: m, ...`.
- Registrar cierre y **salir de la app**.
- Camión **cerrado = bloqueado** (no se pueden cambiar etiquetas).

---

## 7) Modelo de Datos (mínimo viable)

> Nombres ilustrativos; adaptar a tu esquema real.

**USUARIO**
- `IdUsuario (PK)`, `user`, `password`, `activo`, `creadoEn`

**CAMION**
- `IdCamion (PK)`, `Remision`, `Estado`, `FechaPlan`, `CreadoEn`

**TURNO**
- `IdTurno (PK)`, `IdCamion (FK)`, `Usuario (FK)`, `Inicio`, `Fin`, `Duracion`, `Estado`

**ETIQUETA**
- `Cod_Etiqueta (PK)`, `Cod_Item (FK t120_mc_items)`, `...`

**ETIQUETA_LINER**
- `Cod_Etiqueta_Liner (PK)`, `Item (FK t120_mc_items)`, `...`

**t120_mc_items**
- `F120_ID (PK)`, `F120_DESCRIPCION`, `F120_ID_CIA`, `...`

**KARDEX_BODEGA**
- `IdKardex (PK)`, `Etiqueta`, `IdBodega`, `Area`, `FechaEntrada`, `FechaSalida`, `TipoEntrada`, `UbicacionAnterior`, `AreaAnterior`

**ALISTAMIENTO_ETIQUETA**
- `Id (PK)`, `IdCamion (FK)`, `Etiqueta`, `AreaVieja`, `AreaFinal`, `IdBodegaInicial`, `IdBodegaFinal`, `Fecha`

**ERRORES_ALISTAMIENTO**
- `Id (PK)`, `IdTurno (FK)`, `Etiqueta`, `CodigoError` (`DUP`, `N.E`, `N.P`, `SIN_CUPO`, ...), `Mensaje`, `Fecha`

---

## 8) Reglas de Negocio (resumen rápido)

- 1 etiqueta = 1 **paca**.
- **Ventana operativa** para reportes: 7:00 AM a 7:00 AM.
- **Áreas** y **bodegas** deben ser consistentes entre vistas y Kardex.
- Todo error **se persiste**.
- Cualquier cambio de ubicación **se audita**.

---

## 9) Dapper (contratos sugeridos)

> Endpoints/queries orientativos para WinForms + .NET + Dapper.

- `UsuarioRepo.Login(user, pass)`
- `CamionRepo.ListarActivos()`
- `TurnoRepo.Iniciar(idCamion, idUsuario)`
- `TurnoRepo.Cerrar(idTurno)`
- `EtiquetaRepo.ValidarFormato(etiqueta)`
- `EtiquetaRepo.BuscarEnMaestros(etiqueta)`  
  (unificar `ETIQUETA` y `ETIQUETA_LINER`, traer `ITEM`)
- `KardexRepo.Existe(etiqueta)`
- `KardexRepo.InsertarIngreso(etiqueta, idBodega=1, area='ALISTAMIENTO')`
- `KardexRepo.CambiarUbicacion(etiqueta, idBodega=1, areaNueva)`
- `AlistamientoRepo.InsertarMovimiento(idCamion, etiqueta, areaVieja, areaFinal, bodegaIni, bodegaFin)`
- `ErroresRepo.Registrar(idTurno, etiqueta, codigo, mensaje)`
- `ReportesRepo.*` (vistas 4.x y 5.x)
- `ExportService.Excel(dataset)`

---

## 10) Validaciones (SQL/Servidor)

- **Formato etiqueta**: `LEN(@etiqueta) = 10 AND @etiqueta LIKE '%[A-Za-z]%'`
- **Existencia maestro**: buscar en `ETIQUETA` **o** `ETIQUETA_LINER`.
- **Join item** (dos joins, misma t120):
  - `ETIQUETA.COD_ITEM -> t120_mc_items.F120_ID (cia=2)`
  - `ETIQUETA_LINER.ITEM -> t120_mc_items.F120_ID (cia=2)`
- **Pedido vs Progreso**: no superar requeridos, no admitir si `restante = 0`.

---

## 11) UX (WinForms)

- Escaneo en **focus** siempre.
- Indicadores: `Cronómetro`, `Bultos faltantes`, `Errores del turno`.
- Botones clave: `Imprimir`, `Exportar Excel`, `Reset`, `Rehacer Cambios`, `Cerrar Cargue`.

---

## 12) Mensajes tipo (claros y cortos)

- `DUP: {etiqueta}` — Duplicada.
- `N.E – Etiqueta: {etiqueta}` — No existe (bloquea).
- `N.P: {etiqueta}` — No pertenece al pedido.
- `SIN CUPO: {item}` — Requerido completo.
- `CARGUE INCOMPLETO` — Falta(n) bulto(s). Confirmar para cerrar con observaciones.

---
**Fin.** Si algo no está aquí, no existe; si existe y no se mide, no mejora 😉
