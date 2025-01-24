using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityFrameworkCore.Configuration;

public static class ComplexPropertyConfigurationExtension
{
    public static ComplexPropertyBuilder<TComplexPropertyType> Configure<TComplexPropertyType>(
        this ComplexPropertyBuilder<TComplexPropertyType> builder,
        IComplexPropertyConfiguration<TComplexPropertyType> complexPropertyConfiguration,
        params int[] columnOrders) where TComplexPropertyType : class
    {
        complexPropertyConfiguration.Configure(builder, columnOrders);

        return builder;
    }
}
