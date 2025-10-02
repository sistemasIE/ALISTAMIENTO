using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALISTAMIENTO_IE.Models
{
    [Table("DETALLE_CAMION_X_DIA", Schema = "SIE")]
    public partial class DetalleCamionXDia
    {
        [Key]
        [Column("COD_DETALLE_CAMION")]
        public int CodDetalleCamion { get; set; }

        [Column("COD_CAMION")]
        public int CodCamion { get; set; }

        [Column("ITEM")]
        public int Item { get; set; }


        [Column("ITEM_EQUIVALENTE")]
        public int ItemEquivalente { get; set; }

        [Column("ESTADO")]
        [StringLength(1)]
        public string Estado { get; set; }

        public float CANTIDAD_PLANIFICADA { get; set; }
    }
}
