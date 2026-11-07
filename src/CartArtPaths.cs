using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CozyCartArt
{
    public enum CartSide
    {
        Front,
        Rear,
    }

    public static class CartArtPaths
    {
        private const string FrontFolder = "front";
        private const string RearFolder = "rear";
        private const string Extension = ".png";

        public static IReadOnlyList<string> Candidates(string root, CartSide side, string gameId, string platform, string country)
        {
            string folder = Path.Combine(root, side == CartSide.Front ? FrontFolder : RearFolder);
            string id = Sanitize(gameId);
            string variant = BuildVariant(platform, country);
            var names = new List<string>();

            if (variant != null)
            {
                names.Add($"{id}_{variant}");
            }

            names.Add(id);

            if (side == CartSide.Rear && variant != null)
            {
                names.Add(variant);
            }

            return names.Select(name => Path.Combine(folder, name + Extension)).ToList();
        }

        public static string FirstExisting(IEnumerable<string> candidates, Func<string, bool> exists)
        {
            return candidates.FirstOrDefault(exists);
        }

        private static string BuildVariant(string platform, string country)
        {
            if (string.IsNullOrWhiteSpace(platform) || string.IsNullOrWhiteSpace(country))
            {
                return null;
            }

            return $"{Sanitize(platform)}_{Sanitize(country)}";
        }

        private static string Sanitize(string value)
        {
            var kept = (value ?? string.Empty).Where(char.IsLetterOrDigit);

            return new string(kept.ToArray()).ToLowerInvariant();
        }
    }
}
