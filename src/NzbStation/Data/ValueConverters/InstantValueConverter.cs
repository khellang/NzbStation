using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace NzbStation.Data.ValueConverters
{
    public class InstantValueConverter : ValueConverter<Instant, long>
    {
        public static readonly InstantValueConverter Instance = new InstantValueConverter();
        
        private InstantValueConverter() : base(x => x.ToUnixTimeMilliseconds(), x => Instant.FromUnixTimeMilliseconds(x))
        {
        }
    }
}