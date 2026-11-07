using System.Collections.Generic;
using System.IO;
using BepInEx;
using HarmonyLib;

namespace CozyCartArt
{
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "local.cozygame.cartart";
        public const string PluginName = "Cozy Cart Art";
        public const string PluginVersion = "1.0.0";

        private const string ArtFolderName = "CozyCartArt";
        private const string TitlesFileName = "titles.txt";
        private const string TitlesTemplate =
            "# One override per line: <game id> = <new title>, e.g.\n" +
            "# game001 = My Favourite Cartridge\n";

        internal static BepInEx.Logging.ManualLogSource Log;
        internal static string ArtRoot { get; private set; }

        private void Awake()
        {
            Log = Logger;
            ArtRoot = Path.Combine(Paths.ConfigPath, ArtFolderName);
            EnsureFolders();

            new Harmony(PluginGuid).PatchAll();
            Log.LogInfo($"{PluginName} loaded, art folder: {ArtRoot}");
        }

        internal static IReadOnlyDictionary<string, string> LoadTitles()
        {
            string path = Path.Combine(ArtRoot, TitlesFileName);

            return TitleOverrides.Parse(File.ReadAllLines(path));
        }

        private static void EnsureFolders()
        {
            Directory.CreateDirectory(Path.Combine(ArtRoot, "front"));
            Directory.CreateDirectory(Path.Combine(ArtRoot, "rear"));

            string titles = Path.Combine(ArtRoot, TitlesFileName);

            if (!File.Exists(titles))
            {
                File.WriteAllText(titles, TitlesTemplate);
            }
        }
    }
}
