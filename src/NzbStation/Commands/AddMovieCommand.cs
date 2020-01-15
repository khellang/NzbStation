using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NzbStation.Data;
using NzbStation.Data.Entities;
using NzbStation.Models;
using NzbStation.Tmdb;
using Zynapse;

namespace NzbStation.Commands
{
    public class AddMovieCommand : ICommand<MovieDetailsModel>
    {
        public AddMovieCommand(int id)
        {
            Id = id;
        }

        public int Id { get; }

        public class Handler : ICommandHandler<AddMovieCommand, MovieDetailsModel>
        {
            public Handler(TmdbClient client, Database database)
            {
                Client = client;
                Database = database;
            }

            private TmdbClient Client { get; }

            private Database Database { get; }

            public async Task<MovieDetailsModel> HandleAsync(AddMovieCommand command, CancellationToken cancellationToken)
            {
                var movie = await Database.Movies.FirstOrDefaultAsync(x => x.Id.Equals(command.Id), cancellationToken);

                if (movie is null)
                {
                    var tmdbMovie = await Client.GetMovieDetailsAsync(command.Id, cancellationToken);

                    if (tmdbMovie is null)
                    {
                        return null;
                    }

                    movie = new Movie
                    {
                        Id = tmdbMovie.Id,
                        Overview = tmdbMovie.Overview,
                        Title = tmdbMovie.Title,
                        Slug = tmdbMovie.Title,
                        OriginalTitle = tmdbMovie.OriginalTitle,
                        ReleaseDate = tmdbMovie.ReleaseDate,
                        SortTitle = tmdbMovie.Title,
                        Tagline = tmdbMovie.Tagline,
                        Homepage = tmdbMovie.Homepage,
                        ImdbId = tmdbMovie.ImdbId,
                    };

                    foreach (var genre in tmdbMovie.Genres)
                    {
                        movie.Genres.Add(new MovieGenre
                        {
                            MovieId = tmdbMovie.Id,
                            GenreId = genre.Id,
                        });
                    }

                    await Database.Movies.AddAsync(movie, cancellationToken);

                    await Database.SaveChangesAsync(cancellationToken);
                }

                return movie.MapToModel();
            }
        }
    }
}