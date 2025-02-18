public static partial class Excel
{
    /// <summary>
    /// Чтение Excel
    /// </summary>
    public static class Reader
    {
        static Reader() => ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        public static DataTable ReadDataTableFromExcelPackage(ExcelPackage excelPackage, string sheetName, params string[] columnNames)
        {
            var sheet = !string.IsNullOrEmpty(sheetName)
                ? excelPackage.Workbook.Worksheets.FirstOrDefault(s => s.Name == sheetName) ?? excelPackage.Workbook.Worksheets.FirstOrDefault()
                : excelPackage.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
            {
                throw new Exception("Не найден Лист");
            }

            DataTable dt = ReadDataTableFromExcelWorkSheet(sheetName, sheet, columnNames);

            TrimStrings(dt);

            return dt;
        }

        private readonly record struct Column(string Name, int Index, Type Type);

        private static DataTable ReadDataTableFromExcelWorkSheet(string tableName, ExcelWorksheet excelWorksheet, params string[] columnNames)
        {
            DataTable dt = new DataTable();

            dt.TableName = tableName ?? "Table";

            List<Column> columnIndexes = new List<Column>();

            var columns = excelWorksheet.Cells[
                excelWorksheet.Dimension.Start.Row,
                excelWorksheet.Dimension.Start.Column,
                excelWorksheet.Dimension.Start.Row,
                excelWorksheet.Dimension.End.Column].ToArray();

            for (int i = 0; i < columns.Length; i++)
            {
                ExcelRangeBase cell = columns[i];

                string columnName = cell.Text.Trim();

                if (string.IsNullOrEmpty(columnName) == false)
                {
                    Type columnType = excelWorksheet.Cells[
                        excelWorksheet.Dimension.Start.Row + 1,//Отступ шапки
                        excelWorksheet.Dimension.Start.Column + i,
                        excelWorksheet.Dimension.End.Row,
                        excelWorksheet.Dimension.Start.Column + i
                        ]//Выбрать все строки по колонке
                        .Where(x => x.Value != null)
                        .Take(10)
                        .Select(x => x.Value.GetType())
                        .GroupBy(type => type)
                        .OrderByDescending(group => group.Count())
                        .Select(group => group.Key)
                        .FirstOrDefault()
                        ?? typeof(string);

                    columnIndexes.Add(new Column(columnName, excelWorksheet.Dimension.Start.Column + i, columnType));
                }
            }

            if (columnNames.Length > 0)
            {
                var columnNamesNotFounded = columnNames
                    .Where(columnName => columnIndexes.Any(ci => ci.Name == columnName) == false)
                    .Select(s => $"\"{s}\"")
                    .ToArray();

                if (columnNamesNotFounded.Length > 0)
                {
                    throw new Exception($@"Не найдены столбцы в Excel файле:
{string.Join("\n", columnNamesNotFounded)}
");
                }

                columnIndexes = columnIndexes
                    .Where(ci => columnNames.Contains(ci.Name))
                    .ToList();

                foreach (var column in columnIndexes)
                {
                    dt.Columns.Add(column.Name, column.Type);
                }
            }
            else
            {
                foreach (var column in columnIndexes)
                {
                    dt.Columns.Add(column.Name, column.Type);
                }
            }

            for (int r = excelWorksheet.Dimension.Start.Row + 1; r <= excelWorksheet.Dimension.End.Row; r++)
            {
                var values = columnIndexes
                    .Select(column => excelWorksheet.Cells[r, column.Index, r, column.Index].Value)
                    .ToArray();

                if (values.Any(v => v != null))
                {
                    DataRow row = dt.NewRow();

                    row.ItemArray = values;

                    dt.Rows.Add(row);
                }

            }

            return dt;
        }

        private static void TrimStrings(DataTable dt)
        {
            var stringColumns = dt.Columns.OfType<DataColumn>().Where(x => x.DataType == typeof(string)).ToList();

            if (stringColumns.Count == 0) return;

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn dataColumn in stringColumns)
                {
                    row[dataColumn] = row.IsNull(dataColumn) ? null : row.Field<string>(dataColumn)?.Trim();
                }
            }

        }
    }
}
