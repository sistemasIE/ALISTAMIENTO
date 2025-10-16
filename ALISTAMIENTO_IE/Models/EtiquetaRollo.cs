namespace ALISTAMIENTO_IE.Models
{
    public class EtiquetaRollo
    {
        public string CodEtiquetaRollo { get; set; }           // nchar(10)
        public string CodBarras { get; set; }                  // nchar(30)
        public DateTime Fecha { get; set; }                    // datetime
        public int Item { get; set; }                          // int
        public short Telar { get; set; }                       // smallint
        public int Tejedor { get; set; }                       // int
        public decimal PesoBruto { get; set; }                 // numeric(7,2)
        public decimal PesoNeto { get; set; }                  // numeric(7,2)
        public byte Estado { get; set; }                       // tinyint
        public string? CodTipoEtiquetado { get; set; }         // char(2), permite nulos
        public int? Metros { get; set; }                       // int, permite nulos
        public string? EstadoPaIpt { get; set; }               // char(1), permite nulos
        public string? CiOperador { get; set; }                // char(10), permite nulos
        public string? Turno { get; set; }                     // varchar(2), permite nulos
        public long? ConsumidaEn { get; set; }                 // bigint, permite nulos
        public long? DespachadaEn { get; set; }                // bigint, permite nulos
    }
}
