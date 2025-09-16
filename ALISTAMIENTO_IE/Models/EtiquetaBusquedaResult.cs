using ALISTAMIENTO_IE.Model;

namespace ALISTAMIENTO_IE.Models
{
    public class EtiquetaBusquedaResult
    {
        public bool EsValida { get; set; }
        public bool Existe { get; set; }
        public bool ExisteEnKardex { get; set; }
        public string TipoEtiqueta { get; set; } // "ETIQUETA" o "ETIQUETA_LINER"
        public Etiqueta Etiqueta { get; set; }
        public EtiquetaLiner EtiquetaLiner { get; set; }
        public string Mensaje { get; set; }
    }
}
