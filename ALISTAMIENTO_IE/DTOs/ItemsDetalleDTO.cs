namespace ALISTAMIENTO_IE.DTOs
{
    public class ItemsDetalleDTO
    {
        public int Item { get; set; }
        public string Descripcion { get; set; }
        public float CantidadPlanificada { get; set; }
        public int CantidadAlistada { get; set; }
        public double PesoAlistado { get; set; }
        public int DiferenciaUnidades { get; set; }
    }
}
