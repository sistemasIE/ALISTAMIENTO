namespace ALISTAMIENTO_IE.Models
{
    /// <summary>
    /// Modelo que representa un camión en la base de datos [SIE].dbo.CAMION
    /// </summary>
    public class Camion
    {

        public long COD_CAMION { get; set; }

        public string? PLACAS { get; set; }
           
        public byte? TIPOLOGIA { get; set; }
    }
}
