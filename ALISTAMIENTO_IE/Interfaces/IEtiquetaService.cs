using ALISTAMIENTO_IE.DTOs;
using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IEtiquetaService
    {
        Task<Etiqueta?> ObtenerEtiquetaPorCodigoAsync(string codigoEtiqueta);

        Task<EtiquetaBusquedaResult> ValidarEtiquetaEnKardexAsync(string codigoEtiqueta);
    }
}
