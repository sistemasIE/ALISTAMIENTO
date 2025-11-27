using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALISTAMIENTO_IE.Models
{
    // El modelo de la clase se ha ajustado para el mapeo con Dapper.
    // Dapper mapea las propiedades de la clase directamente a las columnas de la tabla.
    [Table("ETIQUETA_LINER")]
    public partial class EtiquetaLiner
    {
        [Key]
        [Column("COD_ETIQUETA_LINER")]
        public string COD_ETIQUETA_LINER { get; set; }

        [Column("CANTIDAD")]
        public int CANTIDAD { get; set; }

        [Column("COD_AREA_x_TIPO_DS")]
        public int COD_AREA_x_TIPO_DS { get; set; }

        [Column("COD_BARRAS")]
        [StringLength(30)]
        public string COD_BARRAS { get; set; }

        [Column("COD_TIPO_ETIQUETADO")]
        [StringLength(2)]
        public string COD_TIPO_ETIQUETADO { get; set; }

        [Column("CONSUMIDAEN")]
        public int CONSUMIDAEN { get; set; }

        [Column("DESPACHADAEN")]
        public int DESPACHADAEN { get; set; }

        [Column("ESTADO")]
        public int ESTADO { get; set; }

        [Column("ESTADO_PA_IPT")]
        [StringLength(1)]
        public string ESTADO_PA_IPT { get; set; }

        [Column("EXT2")]
        public int EXT2 { get; set; }

        [Column("EXTRUSORA")]
        public int EXTRUSORA { get; set; }

        [Column("FECHA")]
        public DateTime FECHA { get; set; }

        [Column("ITEM")]
        public int ITEM { get; set; }

        [Column("OPERADOR")]
        public int OPERADOR { get; set; }

        [Column("PESO_BRUTO")]
        public decimal PESO_BRUTO { get; set; }

        [Column("PESO_NETO")]
        public decimal PESO_NETO { get; set; }

        [Column("SELLADORA")]
        public int SELLADORA { get; set; }

        [Column("TURNO")]
        [StringLength(1)]
        public string TURNO { get; set; }
    }
}
