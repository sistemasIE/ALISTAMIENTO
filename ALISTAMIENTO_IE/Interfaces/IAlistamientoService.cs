using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Models;
using ALISTAMIENTO_IE.Services;
using System.Data;

public interface IAlistamientoService
{
    // --- CONSULTAS PRINCIPALES ---


    Task<Object> ObtenerAlistamiento(int idAlistamiento);
    Task<Alistamiento> ObtenerAlistamientoPorCamionDia(int idCamionDia);

    Task<IEnumerable<CamionItemsDto>> ObtenerItemsPorAlistarCamion(int camionId);
    IEnumerable<CamionEnAlistamientoDTO> ObtenerCamionesEnAlistamiento();
    IEnumerable<Alistamiento> ObtenerAlistamientosActivosOrdenados();
    IEnumerable<DetalleCamionXDia> ObtenerDetallesPorAlistamientosActivos(DetalleCamionXDiaService detalleService);
    Alistamiento ObtenerAlistamientoPorCodCamionYEstado(int idCamionDia, string estado);

    // --- REPORTES / CÁLCULOS ---
    ReporteImpresionTotalesDto CalcularTotalesReporte(IEnumerable<CamionItemsDto> items);
    void AgregarFilaTotalesADataTable(DataTable dataTable, ReporteImpresionTotalesDto totales);

    // --- CRUD OPERACIONES DE ALISTAMIENTO ---
    int InsertarAlistamiento(int idCamionDia, int idUsuario);
    void ActualizarAlistamiento(int idAlistamiento, string nuevoEstado, string observaciones, DateTime? fechaFin);

    // --- MÉTODOS UTILITARIOS (Extensión futura) ---
    Task<bool> ExisteAlistamientoActivo(int idCamionDia);
    Task CargarCamionDia(int idCamion, DataGridView dgv);

}
