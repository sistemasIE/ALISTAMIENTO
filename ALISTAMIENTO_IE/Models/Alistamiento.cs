public partial class Alistamiento
{
    public int IdAlistamiento { get; set; }
    public int IdCamionDia { get; set; }
    public int IdUsuario { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public string Observaciones { get; set; }
    public string Estado { get; set; }
}