using System.Globalization;
using System.Text;

namespace NzbStation.Utilities
{
    public static class Parser
    {
        public static string NormalizeTitle(string title)
        {
            return NormalizeUnicode(title.Trim()).ToLowerInvariant();
        }

        public static string SlugifyTitle(string title)
        {
            return NormalizeTitle(title).Replace(" ", "-");
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
