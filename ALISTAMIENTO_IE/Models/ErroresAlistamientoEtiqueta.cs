using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ALISTAMIENTO_IE.Models;

[Table("ERRORES_ALISTAMIENTO_ETIQUETA")]
public partial class ErroresAlistamientoEtiqueta
{
    [Key]
    [Column("idErrorAlistamientoEtiqueta")]
    public int IdErrorAlistamientoEtiqueta { get; set; }

    [Column("idAlistamiento")]
    public int IdAlistamiento { get; set; }

    [Column("etiqueta")]
    [StringLength(10)]
    public string Etiqueta { get; set; }

    [Column("tipoError")]
    [StringLength(50)]
    public string TipoError { get; set; }
}