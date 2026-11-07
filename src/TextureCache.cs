using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CozyCartArt
{
    // Loaded textures live for the whole session
    internal static class TextureCache
    {
        private static readonly Dictionary<string, Texture2D> Loaded = new Dictionary<string, Texture2D>();

        public static Texture2D Load(string path)
        {
            if (Loaded.TryGetValue(path, out Texture2D cached) && cached != null)
            {
                return cached;
            }

            Texture2D texture = ReadFile(path);
            Loaded[path] = texture;

            return texture;
        }

        private static Texture2D ReadFile(string path)
        {
            var texture = new Texture2D(2, 2, TextureFormat.RGBA32, true);

            if (texture.LoadImage(File.ReadAllBytes(path), true))
            {
                texture.name = Path.GetFileNameWithoutExtension(path);

                return texture;
            }

            Plugin.Log.LogWarning($"Could not decode image, ignored: {path}");
            Object.Destroy(texture);

            return null;
        }
    }
}
