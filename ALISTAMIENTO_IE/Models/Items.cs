using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALISTAMIENTO_IE.Models
{
    [Table("t120_mc_items", Schema = "dbo")]
    public class Items
    {
        [Key]
        [Column("f120_id")]
        public int Id { get; set; }

        [Column("f120_rowid")]
        public int RowId { get; set; }

        [Column("f120_descripcion")]
        public string Descripcion { get; set; }

        [Column("f120_descripcion_corta")]
        public string DescripcionCorta { get; set; }
    }
}
