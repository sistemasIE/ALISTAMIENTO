namespace ALISTAMIENTO_IE.Interfaces
{
    internal interface IPrinterService
    {
        public void PrintDocument(string documentPath);

        public void PrintPDF(string printerName, string documentPath);
    }
}
