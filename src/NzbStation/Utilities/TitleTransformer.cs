using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace NzbStation.Utilities
{
    public static class TitleTransformer
    {
        private static readonly Regex WordDelimiterRegex = new Regex(@"(\s|\.|,|_|-|=|\|)+", RegexOptions.Compiled);

        private static readonly Regex PunctuationRegex = new Regex(@"[^\w\s]", RegexOptions.Compiled);

        private static readonly Regex CommonWordRegex = new Regex(@"\b(a|an|the|and|or|of)\b\s?", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        private static readonly Regex DuplicateSpacesRegex = new Regex(@"\s+", RegexOptions.Compiled);

        public static string Normalize(string title)
        {
            title = WordDelimiterRegex.Replace(title, " ");
            title = PunctuationRegex.Replace(title, string.Empty);
            title = CommonWordRegex.Replace(title, string.Empty);
            title = DuplicateSpacesRegex.Replace(title, " ");

            return title.Trim().ToLower();
        }

        public static string Slugify(string title)
        {
            title = Normalize(title);

            title = NormalizeUnicode(title);

            return title.Replace(' ', '-');
        }

        private static string NormalizeUnicode(string title)
        {
            var builder = new StringBuilder();

            foreach (var @char in title.Normalize(NormalizationForm.FormD))
            {
                var category = CharUnicodeInfo.GetUnicodeCategory(@char);

                if (category != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(@char);
                }
            }

            return builder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
