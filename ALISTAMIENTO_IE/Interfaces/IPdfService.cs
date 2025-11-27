using System.Data;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IPdfService
    {
        public void Generate(DataTable dt1, DataTable dt2, List<string> titles, string outputPath = "C:\\temp\\reporte_final_dt.pdf");

        public void Generate(DataGridView dgv1, DataGridView bigDgv, string title = "Documento", string outputPath = "C:\\temp\\reporte_final.pdf");
        public void Generate(DataTable dataTable, DataGridView bigDgv, string title = "Documento", string outputPath = "C:\\temp\\reporte_final.pdf");

    }
}
