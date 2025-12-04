using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Models;
using ALISTAMIENTO_IE.Services;
using System.Data;

public interface IAlistamientoService
{

    Alistamiento ObtenerAlistamientoPorCodCamionYEstado(int idCamionDia, string estado);

    Task<IEnumerable<Alistamiento>> ObtenerAlistamientosRealizadosPorFecha(DateTime fechaInicio);
    IEnumerable<Alistamiento> ObtenerAlistamientosActivosOrdenados();
    IEnumerable<CamionEnAlistamientoDTO> ObtenerCamionesEnAlistamiento();
    IEnumerable<DetalleCamionXDia> ObtenerDetallesPorAlistamientosActivos(DetalleCamionXDiaService detalleService);
    int InsertarAlistamiento(int idCamionDia, int idUsuario);
    ReporteImpresionTotalesDto CalcularTotalesReporte(IEnumerable<CamionItemsDto> items);
    Task CargarCamionDia(int idCamion, DataGridView dgv);
    Task<Alistamiento> ObtenerAlistamiento(int idAlistamiento);
    Task<Alistamiento> ObtenerAlistamientoPorCamionDia(int idCamionDia);
    Task<bool> ExisteAlistamientoActivo(int idCamionDia);
    Task<IEnumerable<CamionItemsDto>> ObtenerItemsPorAlistarCamion(int camionId);
    Task<List<ReporteTrazabilidadDto>> ObtenerReporteTrazabilidad(IEnumerable<int> codCamiones);
    void ActualizarAlistamiento(int idAlistamiento, string nuevoEstado, string observaciones, DateTime? fechaFin);
    void AgregarFilaTotalesADataTable(DataTable dataTable, ReporteImpresionTotalesDto totales);

}
