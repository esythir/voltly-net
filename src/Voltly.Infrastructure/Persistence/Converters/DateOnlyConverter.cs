using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Voltly.Infrastructure.Persistence.Converters;

public sealed class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter()
        : base(d => d.ToDateTime(TimeOnly.MinValue),
            dt => DateOnly.FromDateTime(dt)) { }
}