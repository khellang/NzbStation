using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NzbStation.Data;
using NzbStation.Data.Entities;
using NzbStation.Tmdb;

namespace NzbStation.Extensions
{
    public static class HostExtensions
    {
        public static async Task MigrateDatabaseAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            using var scope = host.Services.CreateScope();

            await using var context = scope.ServiceProvider.GetRequiredService<Database>();

            await context.Database.EnsureDeletedAsync(cancellationToken);
            await context.Database.EnsureCreatedAsync(cancellationToken);

            var client = scope.ServiceProvider.GetRequiredService<TmdbClient>();

            foreach (var tmdbGenre in await client.GetMovieGenresAsync(cancellationToken))
            {
                var genre = new Genre
                {
                    Id = tmdbGenre.Id,
                    Name = tmdbGenre.Name,
                };

                await context.Genres.AddAsync(genre, cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);

            // await context.Database.MigrateAsync(cancellationToken);
        }
    }
}
