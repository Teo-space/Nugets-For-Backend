public static partial class Excel
{
    /// <summary>
    /// Запись DataTable таблицы в лист Excel Package
    /// </summary>
    public static class Writer
    {
        static Writer() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        /// <summary>
        /// Пишем таблицу в Excel файл с шаблоном отступая 1 под заголовки
        /// </summary>
        public static void WriteDataTableToExcelPackageUsingTemplate(ExcelPackage excelPackage,
            string sheetName, DataTable dt, string startFrom = "A2")
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentNullException(nameof(sheetName));
            }

            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets
                .FirstOrDefault(x => x.Name == sheetName)
                ?? excelPackage.Workbook.Worksheets.Add(sheetName);

            excelWorksheet.Cells[startFrom/*"A2"*/].LoadFromDataTable(dt, false);//начинаем с "A2", и не печатаем имена полей
            excelPackage.Save();
        }

        public static void WriteDataTableToExcelPackage(ExcelPackage excelPackage, string sheetName, DataTable dt)
        {
            if (string.IsNullOrEmpty(sheetName))
            {
                throw new ArgumentNullException(nameof(sheetName));
            }
            ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets
                .FirstOrDefault(x => x.Name == sheetName)
                ?? excelPackage.Workbook.Worksheets.Add(sheetName);

            excelWorksheet.Cells["A1"].LoadFromDataTable(dt, true);
            excelPackage.Save();
        }

    }
}
