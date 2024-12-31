using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace DataTableExtensions.Mapping;

public static class ExtToDataTable
{
    private record Entry(PropertyInfo PropertyInfo, ColumnAttribute ColumnAttribute);

    public static Result<DataTable> ToDataTable<T>(this IEnumerable<T> collection)
    {
        IReadOnlyCollection<Entry> properties =
            typeof(T)
            .GetProperties()
            .Select(propertyInfo => new Entry(propertyInfo, propertyInfo.GetCustomAttributes().OfType<ColumnAttribute>().FirstOrDefault()))
            .ToArray();

        IReadOnlyCollection<string> propertiesWithoutColumnAttributeErrors = properties
            .Where(x => x.ColumnAttribute == null)
            .Select(x => $"Свойство '{x.PropertyInfo.Name}' не имеет атрибута ColumnAttribute")
            .ToArray();
        if (propertiesWithoutColumnAttributeErrors.Count > 0)
        {
            return Results.Error<DataTable>($"Свойства не имеют атрибута ColumnAttribute",
                propertiesWithoutColumnAttributeErrors);
        }

        IReadOnlyCollection<Entry> propertiesWithColumnAttr = properties
            .Where(p => p.ColumnAttribute != null)
            .OrderBy(p => p.ColumnAttribute.Order)
            .ToArray();

        DataTable dt = new DataTable();

        foreach (var property in properties)
        {
            dt.Columns.Add(property.ColumnAttribute.Name, property.PropertyInfo.PropertyType);
        }

        foreach (T item in collection)
        {
            var newRow = dt.NewRow();
            foreach (var property in properties)
            {
                string name = property.ColumnAttribute.Name;

                object value = property.PropertyInfo.GetValue(item);

                newRow[name] = value;
            }
            dt.Rows.Add(newRow);
        }

        return Results.Ok(dt);
    }
}
