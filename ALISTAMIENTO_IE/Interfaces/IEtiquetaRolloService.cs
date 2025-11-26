using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IEtiquetaRolloService
    {
        Task<EtiquetaRollo> ObtenerEtiquetaRolloPorCodigoAsync(string codigoEtiqueta);
    }
}
