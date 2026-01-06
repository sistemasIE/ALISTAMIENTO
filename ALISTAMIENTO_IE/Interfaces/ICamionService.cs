using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio CRUD de Camión
    /// </summary>
    public interface ICamionService
    {
        // ========== MÉTODOS EXISTENTES (backwards compatibility) ==========
        
        Camion? GetCamionById(int codCamion);
        Camion? GetCamionByCamionXDiaId(int codCamion);

        // ========== MÉTODOS CRUD COMPLETOS ==========

        /// <summary>
        /// Obtiene todos los camiones
        /// </summary>
        Task<IEnumerable<Camion>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un camión por su código
        /// </summary>
        Task<Camion?> ObtenerPorIdAsync(long codCamion);

        /// <summary>
        /// Busca camiones por placas (búsqueda parcial)
        /// </summary>
        Task<IEnumerable<Camion>> BuscarPorPlacasAsync(string placas);

        /// <summary>
        /// Inserta un nuevo camión
        /// </summary>
        /// <returns>El código del camión insertado</returns>
        Task<long> InsertarAsync(Camion camion);

        /// <summary>
        /// Actualiza un camión existente
        /// </summary>
        Task<bool> ActualizarAsync(Camion camion);

        /// <summary>
        /// Elimina un camión por su código
        /// </summary>
        Task<bool> EliminarAsync(long codCamion);

        /// <summary>
        /// Verifica si existe un camión con las placas especificadas
        /// </summary>
        Task<bool> ExistePlacasAsync(string placas, long? codCamionExcluir = null);
    }
}
