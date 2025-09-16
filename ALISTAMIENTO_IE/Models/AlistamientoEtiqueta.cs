public record AlistamientoEtiqueta(
    int IdAlistamiento,
    string Etiqueta,
    DateTime Fecha,
    string Estado,
    string AreaInicial,
    string AreaFinal,
    int IdBodegaInicial,
    int IdBodegaFinal,
    int IdUsuario
);