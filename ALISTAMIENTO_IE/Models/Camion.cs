namespace ALISTAMIENTO_IE.Models
{
    /// <summary>
    /// Modelo que representa un camión en la base de datos [SIE].dbo.CAMION
    /// </summary>
    public class Camion
    {
        /// <summary>
        /// Código único del camión (PK)
        /// </summary>
        public long COD_CAMION { get; set; }

        /// <summary>
        /// Placas del camión
        /// </summary>
        public string? PLACAS { get; set; }

        /// <summary>
        /// Tipología del camión
        /// </summary>
        public byte? TIPOLOGIA { get; set; }
    }
}
