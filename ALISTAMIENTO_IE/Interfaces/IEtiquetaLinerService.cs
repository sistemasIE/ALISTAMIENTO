using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IEtiquetaLinerService
    {
        Task<EtiquetaLiner?> ObtenerEtiquetaLinerPorCodigoAsync(string codigoEtiqueta);
    }
}
