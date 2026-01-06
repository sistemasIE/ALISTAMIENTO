using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    /// <summary>
    /// Interfaz para el servicio CRUD de Conductor
    /// </summary>
    public interface IConductorService
    {
        /// <summary>
        /// Obtiene todos los conductores
        /// </summary>
        Task<IEnumerable<Conductor>> ObtenerTodosAsync();

        /// <summary>
        /// Obtiene un conductor por su código
        /// </summary>
        Task<Conductor?> ObtenerPorIdAsync(long codConductor);

        /// <summary>
        /// Busca conductores por nombre (búsqueda parcial)
        /// </summary>
        Task<IEnumerable<Conductor>> BuscarPorNombreAsync(string nombre);

        /// <summary>
        /// Busca un conductor por su cédula de identidad
        /// </summary>
        Task<Conductor?> BuscarPorCIAsync(string ci);

        /// <summary>
        /// Inserta un nuevo conductor
        /// </summary>
        /// <returns>El código del conductor insertado</returns>
        Task<long> InsertarAsync(Conductor conductor);

        /// <summary>
        /// Actualiza un conductor existente
        /// </summary>
        Task<bool> ActualizarAsync(Conductor conductor);

        /// <summary>
        /// Elimina un conductor por su código
        /// </summary>
        Task<bool> EliminarAsync(long codConductor);

        /// <summary>
        /// Verifica si existe un conductor con la cédula especificada
        /// </summary>
        Task<bool> ExisteCIAsync(string ci, long? codConductorExcluir = null);
    }
}
