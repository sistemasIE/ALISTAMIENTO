using System.ComponentModel.DataAnnotations.Schema;

namespace ALISTAMIENTO_IE.Models
{
    [Table("ETIQUETA_ROLLO")]
    public class EtiquetaRollo
    {
        [Column("COD_ETIQUETA_ROLLO")]
        public string CodEtiquetaRollo { get; set; }           // nchar(10)
        
        [Column("COD_BARRAS")]
        public string CodBarras { get; set; }                  // nchar(30)
        
        [Column("FECHA")]
        public DateTime Fecha { get; set; }                    // datetime
        
        [Column("ITEM")]
        public int Item { get; set; }                          // int
        
        [Column("TELAR")]
        public short Telar { get; set; }                       // smallint
        
        [Column("TEJEDOR")]
        public int Tejedor { get; set; }                       // int
        
        [Column("PESO_BRUTO")]
        public decimal PesoBruto { get; set; }                 // numeric(7,2)
        
        [Column("PESO_NETO")]
        public decimal PesoNeto { get; set; }                  // numeric(7,2)
        
        [Column("ESTADO")]
        public byte Estado { get; set; }                       // tinyint
        
        [Column("COD_TIPO_ETIQUETADO")]
        public string? CodTipoEtiquetado { get; set; }         // char(2), permite nulos
        
        [Column("METROS")]
        public int? Metros { get; set; }                       // int, permite nulos
        
        [Column("ESTADO_PA_IPT")]
        public string? EstadoPaIpt { get; set; }               // char(1), permite nulos
        
        [Column("CI_OPERADOR")]
        public string? CiOperador { get; set; }                // char(10), permite nulos
        
        [Column("TURNO")]
        public string? Turno { get; set; }                     // varchar(2), permite nulos
        
        [Column("CONSUMIDA_EN")]
        public long? ConsumidaEn { get; set; }                 // bigint, permite nulos
        
        [Column("DESPACHADA_EN")]
        public long? DespachadaEn { get; set; }                // bigint, permite nulos
    }
}
