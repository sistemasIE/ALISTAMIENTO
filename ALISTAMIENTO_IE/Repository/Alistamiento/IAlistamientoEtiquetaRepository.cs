using ALISTAMIENTO_IE.DTOs;

namespace ALISTAMIENTO_IE.Repository.Alistamiento
{
    public interface IAlistamientoEtiquetaRepository
    {
        Task<AlistamientoDetalleDto> ObtenerPorAlistamientoAsync(int idCamionDia);
    }

}
