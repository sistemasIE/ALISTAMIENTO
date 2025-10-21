namespace ALISTAMIENTO_IE.DTOs
{

    public enum TipoProductoEnum
    {
        SACOS, LINER, ROLLO
    }


    public class EtiquetaLeidaDTO
    {
        public string ETIQUETA { get; set; }
        public int? ITEM { get; set; }
        public string DESCRIPCION { get; set; }
        public string AREA { get; set; }
        public DateTime? FECHA { get; set; }
        public TipoProductoEnum DESDE { get; set; }
        public float VALOR { get; set; }

    }
}
