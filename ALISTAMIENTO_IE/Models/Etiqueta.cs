namespace ALISTAMIENTO_IE.Models;

public class Etiqueta
{
    // El nombre de la propiedad debe coincidir exactamente con el nombre de la columna en la DB.
    public string COD_ETIQUETA { get; set; }

    public int CANTIDAD { get; set; }

    public string COD_CLIENTE { get; set; }

    public int COD_CORTADOR { get; set; }

    public int COD_EMPACADOR { get; set; }

    public int COD_ITEM { get; set; }

    public string COD_TIPO_ETIQUETADO { get; set; }

    public string COD_TURNO { get; set; }

    public int CONSUMIDA_EN { get; set; }

    public int DESPACHADA_EN { get; set; }

    public string ESTADO { get; set; }

    public DateTime FECHA { get; set; }

    public string LOTE { get; set; }

    public int ORDEN_PRODUCCION { get; set; }

    public int PESO { get; set; }

    public int PESO_TEORICO { get; set; }
}
