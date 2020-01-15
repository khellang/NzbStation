using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace NzbStation.Data
{
    public class LocalDateValueConverter : ValueConverter<LocalDate, DateTime>
    {
        public static readonly LocalDateValueConverter Instance = new LocalDateValueConverter();

        private LocalDateValueConverter() : base(x => x.ToDateTimeUnspecified(), x => LocalDate.FromDateTime(x))
        {
        }
    }
}