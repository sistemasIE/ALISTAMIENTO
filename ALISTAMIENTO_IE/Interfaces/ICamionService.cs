using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface ICamionService
    {
        Camion? GetCamionById(int codCamion);

        Camion? GetCamionByCamionXDiaId(int codCamion);
    }
}
