using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("UBICACIONES_ALISTAMIENTO")]
public class UbicacionAlistamiento
{
    /// <summary>
    /// ID de la ubicación de alistamiento.
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// Estado de la ubicación (ej. activa/inactiva).
    /// Mapea al tipo de dato BIT en la base de datos.
    /// </summary>
    [Column("estado")]
    public bool Estado { get; set; }

    /// <summary>
    /// Nombre de la ubicación, siguiendo el formato "AL-1-A" hasta "AL-13-C".
    /// </summary>
    [Column("nombre")]
    [StringLength(20)]
    public string Nombre { get; set; }
}