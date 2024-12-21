public static partial class Excel
{
    /// <summary>
    /// DataTable таблице в Excel файл в виде byte[]
    /// </summary>
    public static class BinaryWriter
    {
        static BinaryWriter() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        public static byte[] GetAsBytes(DataTable dt, string sheetName, string startFrom = "A2")
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentNullException(nameof(sheetName));
            }
            using (var templateExcelPackage = new ExcelPackage())
            {
                Excel.Writer.WriteDataTableToExcelPackageUsingTemplate(templateExcelPackage, sheetName, dt, startFrom);

                var bytes = templateExcelPackage.GetAsByteArray();

                return bytes;
            }
        }

        public static byte[] GetAsBytesUsingTemplate(DataTable dt, string sheetName, string templateFilePath, string startFrom = "A2")
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentNullException(nameof(sheetName));
            }
            if (!File.Exists(templateFilePath))
            {
                throw new InvalidOperationException($"Файл шаблона '{templateFilePath}' не существует");
            }

            using (var templateStream = File.OpenRead(templateFilePath))
            using (var templateExcelPackage = new ExcelPackage())
            {
                templateExcelPackage.Load(templateStream);

                Excel.Writer.WriteDataTableToExcelPackageUsingTemplate(templateExcelPackage, sheetName, dt, startFrom);

                var bytes = templateExcelPackage.GetAsByteArray();

                return bytes;
            }
        }

    }
}
