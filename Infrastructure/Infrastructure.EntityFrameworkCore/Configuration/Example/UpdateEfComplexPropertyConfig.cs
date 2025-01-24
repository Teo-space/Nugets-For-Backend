using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EntityFrameworkCore.Configuration.Example;

/// <summary>
/// builder.ComplexProperty(x => x.Update).Configure(new UpdateEfComplexPropertyConfig(), o++, o++);
/// </summary>
internal class UpdateEfComplexPropertyConfig : IComplexPropertyConfiguration<Update>
{
    public void Configure(ComplexPropertyBuilder<Update> builder, params int[] columnOrders)
    {
        builder.IsRequired();

        builder.Property(x => x.Date).IsRequired().HasColumnName("UPDATE_DATE").HasColumnType("timestamp")
            .IsConcurrencyToken();

        builder.Property(x => x.UserId).IsRequired().HasColumnName("UPDATE_USER_ID").HasDefaultValue(1);

        if (columnOrders.Any())
        {
            builder.Property(x => x.Date).HasColumnOrder(columnOrders[0]);
            builder.Property(x => x.UserId).HasColumnOrder(columnOrders[1]);
        }
    }
}
