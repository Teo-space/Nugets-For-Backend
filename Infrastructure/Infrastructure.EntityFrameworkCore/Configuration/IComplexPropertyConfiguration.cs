using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityFrameworkCore.Configuration;

public interface IComplexPropertyConfiguration<TComplexPropertyType> where TComplexPropertyType : class
{
    public void Configure(ComplexPropertyBuilder<TComplexPropertyType> builder, params int[] columnOrders);
}
