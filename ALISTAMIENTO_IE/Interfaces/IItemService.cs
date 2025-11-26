namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IItemService
    {
        Dictionary<int, string> ObtenerDescripcionesItems(IEnumerable<int> itemIds, int idCia);

        string[] ParseItemsFromTextArea(string rawItems);
    }

}
