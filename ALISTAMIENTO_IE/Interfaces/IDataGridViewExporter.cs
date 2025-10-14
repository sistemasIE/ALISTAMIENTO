using System.Data;

namespace ALISTAMIENTO_IE.Interfaces
{
    public interface IDataGridViewExporter
    {
        DataTable ConvertDynamicToDataTable(IEnumerable<dynamic> items);
        DataTable ConvertToDataTable(DataGridView dgv);
        DataTable ToDataTable<T>(IEnumerable<T> list);
        void ExportToCsv(DataGridView dgv, string filePath);
        void ExportToExcel(DataGridView dgv, string filePath);

    }
}
