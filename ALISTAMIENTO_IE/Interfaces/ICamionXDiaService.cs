using ALISTAMIENTO_IE.Services;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface ICamionXDiaService
    {
        Task<List<CamionXDia>> GetAllAsync();
        Task<CamionXDia?> GetByIdAsync(long codCamion);
        Task<IEnumerable<CamionXDia>> ObtenerCamionesDespachadosPorFecha(DateTime fecha);
        Task<IEnumerable<dynamic>> GetByStatusAsync();
        Task<bool> InsertAsync(CamionXDia camion);
        Task<bool> UpdateAsync(CamionXDia camion);
        Task<bool> DeleteAsync(long codCamion);
        Task<bool> ChangeStatusAsync(long codCamion, string newStatus);
        Task<int> AnularCamionAsync(long codCamion);
    }

}
