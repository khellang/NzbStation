using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace NzbStation.Data
{
    public class InstantValueConverter : ValueConverter<Instant, long>
    {
        public static readonly InstantValueConverter Instance = new InstantValueConverter();
        
        private InstantValueConverter() : base(x => x.ToUnixTimeMilliseconds(), x => Instant.FromUnixTimeMilliseconds(x))
        {
        }
    }
}