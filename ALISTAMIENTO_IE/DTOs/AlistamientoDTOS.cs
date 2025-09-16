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
}
