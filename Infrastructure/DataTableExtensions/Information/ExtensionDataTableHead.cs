using System.Data;
using System.Text;

public static class ExtensionDataTableHead
{
    public static string Head(this DataTable dt, int rowsCount)
    {
        if (dt == default) return string.Empty;

        var builder = new StringBuilder();

        builder.Append($@"
TableName: '{dt.TableName}'
");
        builder.Append($@"Columns ({dt.Columns.Count}):
");
        builder.Append(string.Join(", ", dt.Columns.OfType<DataColumn>().Select(x => $"['{x.ColumnName}', ({x.DataType})], ")));

        builder.Append($"\n");

        builder.Append($@"Rows ({dt.Rows.Count}):
");

        foreach (var row in dt.Rows.OfType<DataRow>().Take(rowsCount))
        {
            builder.AppendLine(string.Join(", ", row.ItemArray.Select(x => $"'{x?.ToString()}'")));
        }

        return builder.ToString();
    }
}
