namespace ALISTAMIENTO_IE.Models
{
    /// <summary>
    /// Modelo que representa un conductor en la base de datos [SIE].dbo.CONDUCTOR
    /// </summary>
    public class Conductor
    {
        /// <summary>
        /// Código único del conductor (PK)
        /// </summary>
        public long COD_CONDUCTOR { get; set; }

        /// <summary>
        /// Nombres completos del conductor
        /// </summary>
        public string? NOMBRES { get; set; }

        /// <summary>
        /// Teléfono de contacto del conductor
        /// </summary>
        public string? TELEFONO { get; set; }

        /// <summary>
        /// Cédula de identidad del conductor
        /// </summary>
        public string? CI { get; set; }
    }
}
