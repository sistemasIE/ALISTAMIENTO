using System.Data;


namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IKardexService
    {
        List<int> ObtenerItemsValidos(string entrada, out List<string> itemsInvalidos);
        DataTable ObtenerDatosDeItems(IEnumerable<string> items);
        Task<int> UpdateAsync(KardexBodega kardex);
        Task<KardexBodega?> ObtenerKardexDeEtiqueta(string etiqueta);
    }

}
