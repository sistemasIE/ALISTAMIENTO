namespace ALISTAMIENTO_IE.DTOs
{

    public class AlistamientoDetalleDto
    {
        public int IdAlistamiento { get; set; }
        public int IdCamionDia { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        public List<AlistamientoEtiqueta> Etiquetas { get; set; } = new();
    }



    public class AlistamientoItemDTO
    {
        public string Item { get; set; }
        public string TipoProducto { get; set; }
        public float CantidadAlistada { get; set; }
        public float Total { get; set; }
    }



    // DTO 
    public class CamionItemsDto
    {


        public string Descripcion { get; set; }
        public string Item { get; set; }
        public string Destino { get; set; }
        public string? UNIDAD { get; set; }
        public string? EMB { get; set; }

        public decimal CantTotalPedido { get; set; }
        public decimal? PacasEsperadas { get; set; }
        public decimal? KilosEsperados { get; set; }
    }

    // DTO para los totales calculados del reporte de impresión
    public class ReporteImpresionTotalesDto
    {
        public decimal TotalCantTotalPedido { get; set; }
        public decimal TotalPacasEsperadas { get; set; }
        public decimal TotalKilosEsperados { get; set; }
    }

    public class ReporteAlistamientoPorTurnoDto
    {
        public string usuario { get; set; }
        public int etiquetasPorHora { get; set; }

        public int carrosDespachados { get; set; }

    }


    public class ReporteTrazabilidadDto
    {
        public int CodCamion { get; set; }
        public string Placas { get; set; }
        public string Item { get; set; }
        public decimal Cant_Planificada { get; set; }
        public decimal Cant_Alistada{ get; set; }
        public decimal Cant_Despachada{ get; set; }
        public decimal PlanVsAlistado { get; set; }
        public decimal AlistadoVsDespachado { get; set; }
    }

}
