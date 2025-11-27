namespace ALISTAMIENTO_IE.Models
{
    /// <summary>
    /// Representa un registro de eliminación (auditoría) de una etiqueta en un alistamiento.
    /// </summary>
    public record EliminacionAlistamientoEtiqueta(
        int IdEliminacionAlistamientoEtiqueta,
        int IdAlistamientoEtiqueta,
        DateTime FechaEliminacion,
        int IdUsuarioElimina,
        string? Observaciones
    );
}
