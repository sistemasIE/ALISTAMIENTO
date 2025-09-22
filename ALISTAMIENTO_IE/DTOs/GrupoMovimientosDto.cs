using ALISTAMIENTO_IE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ALISTAMIENTO_IE.DTOs
{
    public class GrupoMovimientosDto
    {
        public DateTime Fecha { get; set; }
        public string EmpresaTransporte { get; set; } = "";
        public long CodCamion { get; set; }
        public long CodConductor { get; set; }

        public List<MovimientoDocumentoDto> Movimientos { get; set; } = new();

        // Helpers agregados
        public int CantLineas => Movimientos.Count;
    }
}
