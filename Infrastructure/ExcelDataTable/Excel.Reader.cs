public static partial class Excel
{
    /// <summary>
    /// Чтение Excel
    /// </summary>
    public static class Reader
    {
        static Reader() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;


        public static DataTable ReadDataTableFromExcelPackage(ExcelPackage excelPackage, string sheetName)
        {
            var sheet = !string.IsNullOrEmpty(sheetName)
                ? excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == sheetName) ?? excelPackage.Workbook.Worksheets.FirstOrDefault()
                : excelPackage.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
            {
                throw new Exception(string.IsNullOrEmpty(sheetName) ? "Workbook.Worksheets Is Empty" : $"WorkSheet '{sheetName}' not found");
            }

            DataTable dt = ReadDataTableFromExcelWorkSheet(sheetName, sheet);
            TrimStrings(dt);
            return dt;
        }

        private static DataTable ReadDataTableFromExcelWorkSheet(string sheetName, ExcelWorksheet excelWorksheet)
        {
            DataTable dt = new DataTable();

            dt.TableName = sheetName ?? "Table";

            //Заполняем имена и типы столбцов
            int columnNameIndex = 1;
            foreach (var cell in excelWorksheet.Cells[1, 1, 1, excelWorksheet.Dimension.End.Column])
            {
                //расчитываем тип колонки по первым строкам таблицы
                var columnType = excelWorksheet.Cells[2, columnNameIndex, excelWorksheet.Dimension.End.Row, columnNameIndex]
                    .Where(x => x.Value != null)
                    .Take(10)
                    .Select(x => x.Value.GetType())
                    .GroupBy(x => x)
                    .OrderByDescending(group => group.Count())
                    .Select(x => x.Key)
                    .FirstOrDefault()
                    ?? typeof(string);
                dt.Columns.Add(cell.Text, columnType ?? typeof(string));

                columnNameIndex++;
            }
            //Заполняем Rows
            for (int rowNum = 2; rowNum <= excelWorksheet.Dimension.End.Row; rowNum++)
            {
                var wsRow = excelWorksheet.Cells[rowNum, 1, rowNum, excelWorksheet.Dimension.End.Column];

                if (wsRow.All(cell => cell.Value == null)) continue;

                DataRow row = dt.NewRow();

                foreach (var cell in wsRow)
                {
                    row[cell.Start.Column - 1] = cell.Value;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        private static void TrimStrings(DataTable dt)
        {
            var stringColumns = dt.Columns.OfType<DataColumn>().Where(x => x.DataType == typeof(string)).ToList();

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn dataColumn in stringColumns)
                {
                    row[dataColumn] = row.IsNull(dataColumn) ? null : row.Field<string>(dataColumn)?.Trim();//IsNull проверяет на System.DbNull
                }
            }

        }
    }
}