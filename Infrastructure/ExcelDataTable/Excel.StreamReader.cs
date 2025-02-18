﻿public static partial class Excel
{
    /// <summary>
    /// Чтение Excel
    /// </summary>
    public static class StreamReader
    {
        static StreamReader() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        public static DataTable ReadDataTableFromStream(Stream stream, string sheetName, params string[] columnName)
        {
            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Load(stream);
                return Excel.Reader.ReadDataTableFromExcelPackage(excelPackage, sheetName, columnName);
            }
        }

        public static DataTable ReadDataTableFromFile(string filePath, string sheetName, params string[] columnName)
        {
            using (var stream = File.OpenRead(filePath))
            {
                return ReadDataTableFromStream(stream, sheetName, columnName);
            }
        }
    }
}
