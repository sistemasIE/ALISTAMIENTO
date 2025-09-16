using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("KARDEX_BODEGA")]
public class KardexBodega
{
    [Key]
    [Column("idKardexBodega")]
    public int IdKardexBodega { get; set; }

    [Column("tipoEntrada")]
    public string TipoEntrada { get; set; }

    [Column("tipoSalida")]
    public string TipoSalida { get; set; }

    [Column("etiqueta")]
    public string Etiqueta { get; set; }

    [Column("idBodega")]
    public int IdBodega { get; set; }

    [Column("fechaIngreso")]
    public DateTime FechaIngreso { get; set; }

    [Column("fechaSalida")]
    public DateTime? FechaSalida { get; set; }

    [Column("enBodega")]
    public bool EnBodega { get; set; }

    [Column("idUsuarioEntrante")]
    public int IdUsuarioEntrante { get; set; }

    [Column("idUsuarioSalida")]
    public int? IdUsuarioSalida { get; set; }

    [Column("idRemision")]
    public string IdRemision { get; set; }

    [Column("area")]
    public string Area { get; set; }
}