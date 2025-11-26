namespace ALISTAMIENTO_IE.DTOs
{
    public class ReporteTotalesDto
    {
        public int TotalUnidades { get; set; }
        public int TotalCamiones { get; set; }
    }

    public record EliminacionAlistamientoEtiquetaDTO(
        int IdEliminacionAlistamientoEtiqueta,
        int IdAlistamientoEtiqueta,
        string EtiquetaTexto,
        DateTime FechaEliminacion,
        int IdUsuarioElimina,
        string NombreUsuarioElimina,
        string? Observaciones
    );
}
