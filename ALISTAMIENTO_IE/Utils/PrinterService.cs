using iTextSharp.text;
using iTextSharp.text.pdf;
namespace ALISTAMIENTO_IE.Utils
{
    internal class PrinterService
    {

        public string GenerarReporteAlistamiento(DataGridView dgvResumen, DataGridView dgvItemsUbicacion)
        {

            Document doc = new Document(PageSize.LETTER, 30, 30, 40, 40);
            string ruta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ReporteCamiones.pdf");

            using (FileStream stream = new FileStream(ruta, FileMode.Create))
            {
                PdfWriter.GetInstance(doc, stream);
                doc.Open();

                // ----------------------------
                // Header
                // ----------------------------
                iTextSharp.text.Font tituloFont = FontFactory.GetFont("Arial", 14, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font fechaFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);

                Paragraph titulo = new Paragraph("a", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                doc.Add(titulo);

                Paragraph fecha = new Paragraph("Fecha: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fechaFont);
                fecha.Alignment = Element.ALIGN_RIGHT;
                doc.Add(fecha);

                doc.Add(new Paragraph(" ")); // Espacio

                // ----------------------------
                // PRIMERA PÁGINA (3 DataGridViews)
                // ----------------------------
                doc.Add(new Paragraph("Tabla 1:", fechaFont));
                doc.Add(ConvertirDataGridViewATabla(dgvResumen));

                doc.Add(new Paragraph("Tabla 2:", fechaFont));
                doc.Add(ConvertirDataGridViewATabla(dgvResumen));

                doc.Add(new Paragraph("Tabla 3:", fechaFont));
                doc.Add(ConvertirDataGridViewATabla(dgvResumen));

                // Salto de página
                doc.NewPage();

                // ----------------------------
                // SEGUNDA PÁGINA (1 DataGridView grande)
                // ----------------------------
                doc.Add(new Paragraph("Tabla Consolidada:", fechaFont));
                PdfPTable tablaGrande = ConvertirDataGridViewATabla(dgvItemsUbicacion);
                tablaGrande.WidthPercentage = 100; // ocupa todo el ancho
                doc.Add(tablaGrande);

                doc.Close();
            }

            return ruta;

        }

        /// <summary>
        /// Imprime un PDF en la impresora predeterminada.
        /// </summary>
        public void ImprimirPDF(string rutaPDF)
        {
            using (var doc = PdfiumViewer.PdfDocument.Load(rutaPDF))
            using (var printDoc = doc.CreatePrintDocument())
            {
                printDoc.PrintController = new System.Drawing.Printing.StandardPrintController();
                printDoc.Print();
            }
        }


        // ----------------------------
        // FUNCION AUXILIAR: convierte DataGridView a PdfPTable
        // ----------------------------
        private PdfPTable ConvertirDataGridViewATabla(DataGridView dgv)
        {
            PdfPTable tabla = new PdfPTable(dgv.Columns.Count);
            tabla.WidthPercentage = 90; // ocupa 90% del ancho
            iTextSharp.text.Font headerFont = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD);
            iTextSharp.text.Font cellFont = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.NORMAL);

            // Encabezados
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                tabla.AddCell(new PdfPCell(new Phrase(col.HeaderText, headerFont))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                });
            }

            // Filas
            foreach (DataGridViewRow row in dgv.Rows)
            {
                if (!row.IsNewRow)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        tabla.AddCell(new Phrase(cell.Value?.ToString() ?? "", cellFont));
                    }
                }
            }

            return tabla;
        }


    }
}
