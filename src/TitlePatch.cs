using System.Collections.Generic;
using HarmonyLib;

namespace CozyCartArt
{
    // Runs inside the HydrateDatabases transaction, right after the game rewrote every title from GameDatabase.json
    [HarmonyPatch(typeof(DatabaseHydrationGames), nameof(DatabaseHydrationGames.ImportFromJson))]
    internal static class TitlePatch
    {
        private const string UpdateTitleSql = "UPDATE games SET title = ? WHERE id = ?";

        private static void Postfix()
        {
            IReadOnlyDictionary<string, string> titles = Plugin.LoadTitles();

            foreach (KeyValuePair<string, string> entry in titles)
            {
                int changed = DatabaseUtility.GetConnection().Execute(UpdateTitleSql, entry.Value, entry.Key);

                if (changed == 0)
                {
                    Plugin.Log.LogWarning($"titles.txt: unknown game id '{entry.Key}'");
                }
            }

            Plugin.Log.LogInfo($"Applied {titles.Count} title override(s)");
        }
    }
}
