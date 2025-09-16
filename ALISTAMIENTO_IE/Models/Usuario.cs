using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("USUARIOS")]
public partial class Usuario
{
    [Key]
    [Column("UsuarioId")]
    public int UsuarioId { get; set; }

    [Column("loginNombre")]
    [StringLength(100)]
    public string LoginNombre { get; set; }

    [Column("PrimerNombre")]
    [StringLength(100)]
    public string PrimerNombre { get; set; }

    [Column("Apellido")]
    [StringLength(100)]
    public string Apellido { get; set; }

    [Column("Contraseña")]
    [StringLength(100)]
    public string Contraseña { get; set; }

    [Column("posicion")]
    [StringLength(100)]
    public string Posicion { get; set; }

    [Column("Email")]
    [StringLength(150)]
    public string Email { get; set; }

    [Column("ID_Area")]
    public int IdArea { get; set; }

    [Column("Estado")]
    [StringLength(1)]
    public string Estado { get; set; }
}