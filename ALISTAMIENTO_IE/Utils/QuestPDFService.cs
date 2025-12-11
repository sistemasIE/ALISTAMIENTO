using ALISTAMIENTO_IE.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Data;
using System.Diagnostics;

public class QuestPDFService : IPdfService
{

    private readonly IDataGridViewExporter _dataGridViewExporter;

    public QuestPDFService(IDataGridViewExporter dataGridViewExporter)
    {
        _dataGridViewExporter = dataGridViewExporter;
    }

    public void Generate(DataTable dt1, DataTable dt2, List<string> titles, string outputPath = "C:\\temp\\reporte_final_dt.pdf")
    {
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));

        var document = Document.Create(container =>
        {
            // Página 1: Información Resumida (usa dt1)
            container.Page(page =>
            {
                page.Margin(40);
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    for(int i = 0; i < titles.Count; i++)
                    {
                        col.Item().Text(titles[i]).FontSize(18).Bold().AlignCenter(); 
                    }
                    col.Item().Text("Reporte de Alistamiento - Información General").FontSize(19).Bold().AlignCenter(); // Reducido de 22 a 19
                    col.Item().Text($"Generado el: {DateTime.Now:dd/MM/yyyy HH:mm}").FontSize(9).Italic(); // Reducido de 10 a 9

                    // Seccion 1 de dt1
                    col.Item().PaddingVertical(5).Text("Tabla de Resumen N°1").FontSize(12).SemiBold().Underline(); // Reducido de 14 a 12
                    col.Item().Element(c => AddDataTableWithCustomColumns(c, dt1));

                    // Seccion 2 de dt1 (se repite para simular las 3 tablas que tenías)
                    col.Item().PaddingVertical(5).Text("Tabla de Resumen N°2").FontSize(12).SemiBold().Underline(); // Reducido de 14 a 12
                    col.Item().Element(c => AddDataTableWithCustomColumns(c, dt1));

                    // Seccion 3 de dt1
                    col.Item().PaddingVertical(5).Text("Tabla de Resumen N°3").FontSize(12).SemiBold().Underline(); // Reducido de 14 a 12
                    col.Item().Element(c => AddDataTableWithCustomColumns(c, dt1));
                });
            });

            // Página 2: Datos Extensos (usa dt2)
            container.Page(page =>
            {
                page.Margin(40);
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    for (int i = 0; i < titles.Count; i++)
                    {
                        col.Item().Text(titles[i]).FontSize(18).Bold().AlignCenter();
                    }
                    col.Item().Text("Listado completo de Registros Detallados").FontSize(18).Bold().AlignCenter(); // Reducido de 20 a 18
                    col.Item().Element(c => AddLargeDataTableWithCustomColumns(c, dt2));
                });
            });
        });

        document.GeneratePdf(outputPath);
        try
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(outputPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al intentar abrir el PDF: {ex.Message}", "Error de Apertura", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void Generate(DataGridView dgv1, DataGridView bigDgv, string title = "Documento", string outputPath = "C:\\temp\\reporte_final.pdf")
    {

        // Crear el directorio si no existe
        Directory.CreateDirectory(Path.GetDirectoryName(outputPath));


        var document = Document.Create(container =>
        {
            // Página 1
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text("Reporte de Alistamiento - Página 1").FontSize(20).Bold().AlignCenter();
                    col.Item().Text("Información general del reporte.").Italic();
                    col.Item().Element(c => AddTable(c, dgv1));
                    col.Item().PaddingVertical(10);
                    col.Item().Element(c => AddTable(c, dgv1));
                    col.Item().PaddingVertical(10);
                    col.Item().Element(c => AddTable(c, dgv1));
                });
            });

            // Página 2 (automáticamente agrega más si el DGV es largo)
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text(title).FontSize(20).Bold().AlignCenter();
                    col.Item().Text("Listado completo de registros").FontSize(20).Bold().AlignCenter();
                    col.Item().Element(c => AddLargeTable(c, bigDgv));
                });
            });
        });

        document.GeneratePdf(outputPath);
        try
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo(outputPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error al intentar abrir el PDF: {ex.Message}", "Error de Apertura", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public void Generate(DataTable dataTable, DataGridView bigDgv, string title = "Documento", string outputPath = "C:\\temp\\reporte_final.pdf")
    {
        var document = Document.Create(container =>
        {
            // Página 1
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text("Reporte de Alistamiento - Página 1").FontSize(20).Bold().AlignCenter();
                    col.Item().Text("Información general del reporte.").Italic();
                    col.Item().Element(c => AddDataTable(c, dataTable));
                    col.Item().PaddingVertical(10);
                    col.Item().Element(c => AddDataTable(c, dataTable));
                    col.Item().PaddingVertical(10);
                    col.Item().Element(c => AddDataTable(c, dataTable));
                });
            });

            // Página 2 (automáticamente agrega más si el DGV es largo)
            container.Page(page =>
            {
                page.Margin(30);
                page.Content().Column(col =>
                {
                    col.Spacing(10);
                    col.Item().Text(title).FontSize(20).Bold().AlignCenter();
                    col.Item().Element(c => AddLargeTable(c, bigDgv));
                });
            });
        });

        document.GeneratePdf(outputPath);
        try
        {
            // Esto le dice al sistema operativo: "Abre este archivo con el programa asociado (.pdf)"
            System.Diagnostics.Process.Start(new ProcessStartInfo(outputPath) { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            // Manejo de error si, por ejemplo, el archivo no se encontró o el OS no tiene app predeterminada.
            // Esto es un buen hábito de ingeniero mecatrónico/informático.
            MessageBox.Show($"Error al intentar abrir el PDF: {ex.Message}", "Error de Apertura", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void AddDataTableWithCustomColumns(IContainer container, DataTable dt)
    {
        if (dt == null || dt.Columns.Count == 0)
        {
            container.Text("No hay datos disponibles para mostrar en esta tabla.").Italic().FontSize(10).AlignCenter(); // Reducido de 12 a 10
            return;
        }

        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    // Si la columna se llama "Descripción" o "Descripcion", hacerla más ancha
                    if (dt.Columns[i].ColumnName.Equals("Descripción", StringComparison.OrdinalIgnoreCase) ||
                        dt.Columns[i].ColumnName.Equals("Descripcion", StringComparison.OrdinalIgnoreCase))
                    {
                        columns.RelativeColumn(3); // 3 veces más ancha que las demás
                    }
                    else
                    {
                        columns.RelativeColumn(1); // Ancho normal
                    }
                }
            });

            // Encabezados
            table.Header(header =>
            {
                foreach (DataColumn col in dt.Columns)
                    header.Cell().Border(1).Padding(3).Text(col.ColumnName).Bold().FontSize(9); // Reducido, sin especificar antes
            });

            // Filas
            foreach (DataRow row in dt.Rows)
            {
                foreach (var value in row.ItemArray)
                {
                    string cellValue = value?.ToString() ?? "";
                    table.Cell().Border(1).Padding(3).Text(cellValue).FontSize(8); // Reducido, sin especificar antes
                }
            }
        });
    }

    private void AddLargeDataTableWithCustomColumns(IContainer container, DataTable dt)
    {
        if (dt == null || dt.Columns.Count == 0)
        {
            container
              .PaddingVertical(10)
              .Text("No hay datos disponibles para mostrar en esta tabla.").Italic().FontSize(10).AlignCenter(); // Reducido de 12 a 10
            return;
        }

        container
          .PaddingVertical(10)
          .Decoration(decoration =>
          {
              // El texto de "Datos extensos:" con tamaño reducido
              decoration.Before().Text("Datos extensos:").FontSize(14).Bold().Underline(); // Reducido de 16 a 14

              decoration.Content().Table(table =>
              {
                  table.ColumnsDefinition(columns =>
                                    {
                                        for (int i = 0; i < dt.Columns.Count; i++)
                                        {
                                            // Si la columna se llama "Descripción" o "Descripcion", hacerla más ancha
                                            if (dt.Columns[i].ColumnName.Equals("Descripción", StringComparison.OrdinalIgnoreCase) ||
                                                dt.Columns[i].ColumnName.Equals("Descripcion", StringComparison.OrdinalIgnoreCase))
                                            {
                                                columns.RelativeColumn(3); // 3 veces más ancha que las demás
                                            }
                                            else
                                            {
                                                columns.RelativeColumn(1); // Ancho normal
                                            }
                                        }
                                    });

                  // Encabezados
                  table.Header(header =>
                  {
                      foreach (DataColumn col in dt.Columns)
                          header.Cell().Border(1).Padding(3).Text(col.ColumnName).Bold().FontSize(9); // Reducido, sin especificar antes
                  });

                  // Filas
                  foreach (DataRow row in dt.Rows)
                  {
                      foreach (var value in row.ItemArray)
                      {
                          string cellValue = value?.ToString() ?? "";
                          table.Cell().Border(1).Padding(3).Text(cellValue).FontSize(8); // Reducido, sin especificar antes
                      }
                  }
              });
          });
    }


    private void AddLargeTable(IContainer container, DataGridView dgv)
    {
        if (dgv == null || dgv.Columns.Count == 0)
        {
            container
                .PaddingVertical(10)
                .Text("No hay datos disponibles para mostrar en esta tabla.").Italic().FontSize(12).AlignCenter();
            return;
        }

        container
            .PaddingVertical(10)
            .Decoration(decoration =>
            {
                decoration.Before().Text("Datos extensos:").FontSize(16).Bold().Underline();

                decoration.Content().Table(table =>
                {
                    // ✅ Definir columnas solo una vez
                    table.ColumnsDefinition(columns =>
                    {
                        for (int i = 0; i < dgv.Columns.Count; i++)
                            columns.RelativeColumn();
                    });

                    // Encabezados
                    table.Header(header =>
                    {
                        foreach (DataGridViewColumn col in dgv.Columns)
                            header.Cell().Border(1).Padding(3).Text(col.HeaderText).Bold();
                    });

                    // Filas
                    foreach (DataGridViewRow row in dgv.Rows)
                    {
                        if (row.IsNewRow) continue;

                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            string value = cell.Value?.ToString() ?? "";
                            table.Cell().Border(1).Padding(3).Text(value);
                        }
                    }
                });
            });
    }

    private void AddDataTable(IContainer container, DataTable dt)
    {
        if (dt == null || dt.Columns.Count == 0)
        {
            container.Text("No hay datos disponibles para mostrar en esta tabla.").Italic().FontSize(12).AlignCenter();
            return;
        }

        container.Table(table =>
        {
            // ✅ Definir columnas solo una vez
            table.ColumnsDefinition(columns =>
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                    columns.RelativeColumn();
            });

            // Encabezados
            table.Header(header =>
            {
                foreach (DataColumn col in dt.Columns)
                    header.Cell().Border(1).Padding(3).Text(col.ColumnName).Bold();
            });

            // Filas
            foreach (DataRow row in dt.Rows)
            {
                foreach (var value in row.ItemArray)
                {
                    string cellValue = value?.ToString() ?? "";
                    table.Cell().Border(1).Padding(3).Text(cellValue);
                }
            }
        });
    }
    private void AddTable(IContainer container, DataGridView dgv)
    {
        if (dgv == null || dgv.Columns.Count == 0)
        {
            container.Text("No hay datos disponibles para mostrar en esta tabla.").Italic().FontSize(12).AlignCenter();
            return;
        }

        container.Table(table =>
        {
            // ✅ Definir columnas una sola vez
            table.ColumnsDefinition(columns =>
            {
                for (int i = 0; i < dgv.Columns.Count; i++)
                    columns.RelativeColumn();
            });

            // Encabezados
            table.Header(header =>
            {
                foreach (DataGridViewColumn col in dgv.Columns)
                    header.Cell().Border(1).Padding(3).Text(col.HeaderText).Bold();
            });

            // Filas
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (row.IsNewRow) continue;

                foreach (DataGridViewCell cell in row.Cells)
                {
                    string value = cell.Value?.ToString() ?? "";
                    table.Cell().Border(1).Padding(3).Text(value);
                }
            }
        });
    }

}
