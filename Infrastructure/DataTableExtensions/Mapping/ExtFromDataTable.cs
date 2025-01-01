using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace DataTableExtensions.Mapping;

public static class ExtFromDataTable
{
    private record Entry(PropertyInfo PropertyInfo, ColumnAttribute ColumnAttribute);

    public static Result<IReadOnlyCollection<TResult>> ToCollection<TResult>(this DataTable dt) where TResult : class, new()
    {
        IReadOnlyCollection<Entry> properties =
            typeof(TResult)
            .GetProperties()
            .Select(propertyInfo => new Entry(propertyInfo, propertyInfo.GetCustomAttributes().OfType<ColumnAttribute>().FirstOrDefault()))
            .ToArray();

        {
            IReadOnlyCollection<string> propertiesWithoutColumnAttributeErrors = properties
                .Where(x => x.ColumnAttribute == null)
                .Select(x => $"Свойство '{x.PropertyInfo.Name}' не имеет атрибута ColumnAttribute")
                .ToArray();
            if (propertiesWithoutColumnAttributeErrors.Count > 0)
            {
                return Results.Error<IReadOnlyCollection<TResult>>($"Свойства выходной коллекции не имеют атрибута ColumnAttribute",
                    propertiesWithoutColumnAttributeErrors);
            }
        }
        {
            IReadOnlyCollection<string> propertiesNotExistsInDataTableColumns = properties
                .Where(p => dt.Columns.OfType<DataColumn>().Any(column => column.ColumnName == p.ColumnAttribute.Name) == false)
                .Select(x => $"Таблица не содержит столбец '{x.ColumnAttribute.Name}' (Property: {x.PropertyInfo.Name})")
                .ToArray();

            if (propertiesNotExistsInDataTableColumns.Count > 0)
            {
                return Results.Error<IReadOnlyCollection<TResult>>($"Таблица не содержит столбецы из выходной коллекции",
                    propertiesNotExistsInDataTableColumns);
            }
        }
        {
            IReadOnlyCollection<string> propertiesTypeNotMatchColumns = properties
                .Select(e => new
                {
                    Entry = e,
                    DataColumn = dt.Columns.OfType<DataColumn>().First(column => column.ColumnName == e.ColumnAttribute.Name)
                })
                .Where(e => e.Entry.PropertyInfo.PropertyType != e.DataColumn.DataType)
                .Select(x =>
$"Тип '{x.DataColumn.DataType.Name}' столбца не соответствует типу свойства '{x.Entry.PropertyInfo.PropertyType.Name}'")
                .ToArray();

            if (propertiesTypeNotMatchColumns.Count > 0)
            {
                return Results.Error<IReadOnlyCollection<TResult>>($"Тип столбца таблицы не соответствует типу свойства выходной коллекции",
                    propertiesTypeNotMatchColumns);
            }
        }

        List<TResult> resultsList = new List<TResult>();

        foreach (DataRow row in dt.Rows)
        {
            TResult resultRow = new TResult();

            foreach (Entry entry in properties)
            {
                object value = row[entry.ColumnAttribute.Name];

                entry.PropertyInfo.SetValue(resultRow, value, null);
            }
            resultsList.Add(resultRow);
        }

        IReadOnlyCollection<TResult> results = resultsList;

        return Results.Ok(results);
    }
}
