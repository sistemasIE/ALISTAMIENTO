using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IEliminacionAlistamientoEtiquetaService
    {
        /// <summary>
        /// Inserta registros de eliminaciones en la tabla ELIMINADAS_ALISTAMIENTO_ETIQUETA.
        /// </summary>
        /// <param name="registros">Colección de registros sin Id (se ignora el IdEliminacionAlistamientoEtiqueta).</param>
        /// <returns>Número de filas insertadas.</returns>
        Task<int> InsertarEliminacionesAsync(IEnumerable<EliminacionAlistamientoEtiqueta> registros);

        /// <summary>
        /// Inserta una sola eliminación con observaciones.
        /// </summary>
        Task<int> InsertarEliminacionAsync(int idAlistamientoEtiqueta, int idUsuarioElimina, string? observaciones);

        /// <summary>
        /// Elimina lógicamente (borra de auditoría) un registro de eliminación si fuera necesario (normalmente no se borra la auditoría).
        /// </summary>
        Task<int> EliminarRegistroAsync(int idEliminacionAlistamientoEtiqueta);

        /// <summary>
        /// Obtiene las eliminaciones registradas para un alistamientoEtiqueta específico.
        /// </summary>
        Task<IEnumerable<EliminacionAlistamientoEtiqueta>> ObtenerEliminacionesPorAlistamientoEtiquetaAsync(int idAlistamientoEtiqueta);

        /// <summary>
        /// Obtiene todas las eliminaciones realizadas sobre un alistamiento dado.
        /// </summary>
        Task<IEnumerable<EliminacionAlistamientoEtiqueta>> ObtenerEliminacionesPorAlistamientoAsync(int idAlistamiento);


        Task<IEnumerable<EliminacionAlistamientoEtiquetaDTO>> ObtenerEliminacionesPorAlistamientoConEtiquetaAsync(int idAlistamiento);

        Task RevertirEliminacionAsync(int idEliminacionRegistro, int idUsuarioReversion);

    }
}



