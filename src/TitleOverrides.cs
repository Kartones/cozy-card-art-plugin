using System;
using System.Collections.Generic;

namespace CozyCartArt
{
    public static class TitleOverrides
    {
        private const char Separator = '=';
        private const char CommentMarker = '#';

        public static IReadOnlyDictionary<string, string> Parse(IEnumerable<string> lines)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (string line in lines)
            {
                if (TryParseLine(line, out string id, out string title))
                {
                    result[id] = title;
                }
            }

            return result;
        }

        private static bool TryParseLine(string line, out string id, out string title)
        {
            id = title = null;
            string trimmed = line.Trim();
            int split = trimmed.IndexOf(Separator);

            if (trimmed.Length == 0 || trimmed[0] == CommentMarker || split < 0)
            {
                return false;
            }

            id = trimmed.Substring(0, split).Trim();
            title = trimmed.Substring(split + 1).Trim();

            return id.Length > 0 && title.Length > 0;
        }
    }
}
