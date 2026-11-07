using System.IO;
using UnityEngine;

namespace CozyCartArt
{
    internal static class CartArtResolver
    {
        public static Texture2D Find(CartSide side, GameItem item)
        {
            var candidates = CartArtPaths.Candidates(Plugin.ArtRoot, side, item.id, item.platform, item.country);
            string path = CartArtPaths.FirstExisting(candidates, File.Exists);

            return path == null ? null : TextureCache.Load(path);
        }
    }
}
