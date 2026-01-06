using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("CAMION_X_DIA", Schema = "dbo")]
public class CamionXDia
{
    [Key]  
    [Column("COD_CAMION")]
    public long CodCamion { get; set; }

    [Column("FECHA")]
    public DateTime? Fecha { get; set; }

    [Column("COD_EMPRESA_TRANSPORTE")]
    public string? CodEmpresaTransporte { get; set; }  // <-- nullable

    [Column("ESTADO")]
    public string? Estado { get; set; } // char(1), pero string funciona

    [Column("COD_REGISTRO_CAMION")]
    public long? CodRegistroCamion { get; set; }

    [Column("COD_CONDUCTOR")]
    public long? CodConductor { get; set; }
}