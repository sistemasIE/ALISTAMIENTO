using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IDetalleCamionXDiaService
    {
        IEnumerable<DetalleCamionXDia> ObtenerPorCodCamion(int codCamion);

        IEnumerable<DetalleCamionXDia> ObtenerPorCodCamionYItem(int codCamion, int item);

        int ActualizarItemDadoCodCamionEItem(int codCamion, int item, int nuevoItem);
    }

}
