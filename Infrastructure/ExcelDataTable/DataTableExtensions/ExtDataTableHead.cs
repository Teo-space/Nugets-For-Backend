using System.Text;

public static class ExtDataTableHead
{
    public static string Head(this DataTable dt)
    {
        if (dt == default) return string.Empty;

        var builder = new StringBuilder();
        builder.Append($@"
TableName: '{dt.TableName}'
");
        builder.Append($@"Columns ({dt.Columns.Count}):
");
        builder.Append(string.Join(", ", dt.Columns.OfType<DataColumn>().Select(x => $"['{x.ColumnName}', ({x.DataType})]")));
        builder.Append($"\n");

        if (dt.Columns.Count > 0)
        {
            builder.Append($@"Rows ({dt.Rows.Count}):
");
            foreach (var row in dt.Rows.OfType<DataRow>().Take(10))
            {
                builder.AppendLine(string.Join(", ", row.ItemArray.Select(x => $"'{x?.ToString()}'")));
            }
        }
        else
        {
            builder.Append($@"Rows ({dt.Rows.Count})");
        }

        return builder.ToString();
    }

}
