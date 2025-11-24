using ALISTAMIENTO_IE.DTOs;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IAlistamientoEtiquetaService
    {
        Task<List<AlistamientoItemDTO>> ObtenerItemAlistados(int idCamionDia);
        Task<IEnumerable<ReporteAlistamientoPorTurnoDto>> ObtenerReportePacasPorHora(DateTime fechaConsulta);

        Task<IEnumerable<dynamic>> ObtenerReportePacasPorHoraPorTurno(DateTime fechaConsulta, string? turnoLike);


        Task<ReporteTotalesDto> ObtenerTotalesReporte(DateTime fechaConsulta, string? turnoLike);

        Task<IEnumerable<PlanificacionCamionDTO>> ObtenerPlanificacionCamionAsync(int codCamion);


        Task<AlistamientoEtiqueta> InsertarAlistamientoEtiquetaAsync(int idAlistamiento, string etiqueta,
            DateTime fecha, string estado, string areaInicial, string areaFinal, int idBodegaInicial, int idBodegaFinal,
            int idUsuario);


        Task<IEnumerable<EtiquetaLeidaDTO>> ObtenerEtiquetasLeidas(int idAlistamiento);

        Task<IEnumerable<AlistamientoEtiqueta>> ObtenerAlistamientoEtiquetaPorEtiquetasYAlistamiento(int idAlistamiento, string[] codigoeEtiquetas);

        Task<int> EliminarEtiquetasDeAlistamiento(int idAlistamiento, string[] codigoeEtiquetas);


    }
}
