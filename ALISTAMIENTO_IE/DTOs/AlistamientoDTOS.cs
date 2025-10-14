namespace ALISTAMIENTO_IE.DTOs
{
    // DTO para manejar los totales de pacas y camiones en un reporte.
    public class ReporteTotalesDto
    {
        public int TotalPacas { get; set; }
        public int TotalCamiones { get; set; }
    }

    // DTO para manejar los ítems restantes en un reporte específico.
    public class ItemRestanteDto
    {
        public int Item { get; set; }
        public string Descripcion { get; set; }
        public int Restantes { get; set; }
    }

    // DTO 
    public class CamionItemsDto
    {


        public DateTime Fecha { get; set; }
        public string Item { get; set; }
        public string Descripcion { get; set; }
        public string? UNIDAD { get; set; }
        public string? EMB { get; set; }
        public string Placas { get; set; }

        public decimal CantTotalPedido { get; set; }
        public decimal? PacasEsperadas { get; set; }
        public decimal? KilosEsperados { get; set; }
    }

}
