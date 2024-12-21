public static partial class Excel
{
    /// <summary>
    /// Чтение Excel
    /// </summary>
    public static class StreamReader
    {
        static StreamReader() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


        public static DataTable ReadDataTableFromStream(Stream stream, string sheetName)
        {
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Load(stream);
                return Excel.Reader.ReadDataTableFromExcelPackage(excelPackage, sheetName);
            }
        }


        public static DataTable ReadDataTableFromFile(string filePath, string sheetName)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return ReadDataTableFromStream(stream, sheetName);
            }
        }


    }
}