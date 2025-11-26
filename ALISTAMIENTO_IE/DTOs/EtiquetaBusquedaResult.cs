using ALISTAMIENTO_IE.Models;

namespace ALISTAMIENTO_IE.DTOs
{

    public class EtiquetaBusquedaResult
    {
        public bool EsValida { get; set; }
        public bool Existe { get; set; }
        public bool ExisteEnKardex { get; set; }
        public string TipoEtiqueta { get; set; } // "ETIQUETA", "ETIQUETA_LINER" o "ETIQUETA_ROLLO"
        public Etiqueta Etiqueta { get; set; }
        public EtiquetaLiner EtiquetaLiner { get; set; }
        public EtiquetaRollo EtiquetaRollo { get; set; }
        public string Mensaje { get; set; }
    }

}
