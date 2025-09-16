namespace ALISTAMIENTO_IE.DTOs
{
    public class CamionDetallesDTO
    {
        public int CodCamion { get; set; }
        public string Placas { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public int CantTotalPedido { get; set; }
    }

    public class CamionEnAlistamientoDTO : CamionDetallesDTO
    {
        public string EstadoAlistamiento { get; set; }

    }
}
