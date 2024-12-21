using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

public static class ExtCollectionToDataTable
{
    /// <summary>
    /// Converts collection to Datatable
    /// Supports ColumnAttribute for change Name and Order
    /// </summary>
    public static DataTable ToDataTable<T>(this IEnumerable<T> collection)
    {
        var propertiesWithColumnAttr =
            typeof(T)
            .GetProperties()
            .Where(p => p.GetCustomAttributes().OfType<ColumnAttribute>().Any())
            .ToArray();

        if (propertiesWithColumnAttr.Any())
        {
            return ToDataTableWithColumnAttribute(collection, propertiesWithColumnAttr);
        }
        else
        {
            return ToDataTableSimple<T>(collection, typeof(T).GetProperties());
        }
    }

    private static DataTable ToDataTableSimple<T>(this IEnumerable<T> collection, IEnumerable<PropertyInfo> properties)
    {
        DataTable dt = new DataTable();

        foreach (var property in properties)
        {
            dt.Columns.Add(property.Name, property.PropertyType);
        }

        foreach (T item in collection)
        {
            var newRow = dt.NewRow();
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                newRow[property.Name] = value;
            }
            dt.Rows.Add(newRow);
        }

        return dt;
    }

    public static DataTable ToDataTableWithColumnAttribute<T>(this IEnumerable<T> collection,
        IReadOnlyCollection<PropertyInfo> propertiesWithColumnAttr)
    {
        DataTable dt = new DataTable();

        var properties = propertiesWithColumnAttr
            .Select(p => new
            {
                PropertyInfo = p,
                ColumnAttribute = p.GetCustomAttributes().OfType<ColumnAttribute>().FirstOrDefault()
            })
            .Where(p => p.ColumnAttribute != null)
            .OrderBy(p => p.ColumnAttribute.Order)
            .ToArray();

        foreach (var property in properties)
        {
            dt.Columns.Add(property.ColumnAttribute.Name ?? property.PropertyInfo.Name, property.PropertyInfo.PropertyType);
        }

        foreach (T item in collection)
        {
            var newRow = dt.NewRow();
            foreach (var property in properties)
            {
                string name = property.ColumnAttribute.Name ?? property.PropertyInfo.Name;

                var value = property.PropertyInfo.GetValue(item);

                newRow[name] = value;
            }
            dt.Rows.Add(newRow);
        }

        return dt;
    }

}
