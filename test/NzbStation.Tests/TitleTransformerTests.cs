using System.Collections.Generic;
using System.Linq;
using NzbStation.Utilities;
using Xunit;

namespace NzbStation.Tests
{
    public class TitleTransformerTests
    {
        public static IEnumerable<object[]> Normalized => MovieTitles.Select(x => new object[] { x.Title, x.Normalized });

        public static IEnumerable<object[]> Slugified => MovieTitles.Select(x => new object[] { x.Title, x.Slugified });

        private static IEnumerable<MovieTitle> MovieTitles
        {
            get
            {
                yield return new MovieTitle("The Joker", "joker", "joker");
            }
        }

        [Theory]
        [MemberData(nameof(Normalized))]
        public void NormalizeReturnsCorrectOutput(string title, string expected)
        {
            Assert.Equal(expected, TitleTransformer.Normalize(title));
        }

        [Theory]
        [MemberData(nameof(Slugified))]
        public void SlugifyReturnsCorrectOutput(string title, string expected)
        {
            Assert.Equal(expected, TitleTransformer.Slugify(title));
        }

        private class MovieTitle
        {
            public MovieTitle(string title, string normalized, string slugified)
            {
                Title = title;
                Normalized = normalized;
                Slugified = slugified;
            }

            public string Title { get; }

            public string Normalized { get; }

            public string Slugified { get; }
        }
    }
}
