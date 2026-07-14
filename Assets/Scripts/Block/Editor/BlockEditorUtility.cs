using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class BlockEditorUtility
{
    static readonly List<string> blockKeys = new List<string> ();
    static readonly Dictionary<string, List<string>> entryKeysTable = new Dictionary<string, List<string>> ();

    public static IReadOnlyList<string> BlockKeys => blockKeys;
    public static IReadOnlyDictionary<string, List<string>> EntryKeysTable => entryKeysTable;

    public static bool TryGetEntryKeys (string blockKey, out List<string> entryKeys)
    {
        if (entryKeysTable.TryGetValue (blockKey, out entryKeys))
        {
            return true;
        }
        else
        {
            entryKeys = null;
            return false;
        }
    }

    public static void Refresh ()
    {
        blockKeys.Clear ();
        entryKeysTable.Clear ();

        var collectors = Object.FindObjectsByType<BlockCollector> (FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var collector in collectors)
        {
            if (collector == null || string.IsNullOrEmpty (collector.key))
            {
                continue;
            }

            if (!entryKeysTable.ContainsKey (collector.key))
            {
                blockKeys.Add (collector.key);
                entryKeysTable.Add (collector.key, new List<string> ());
            }

            List<string> targetEntryKeys = entryKeysTable[collector.key];
            if (collector.Entrys == null)
            {
                continue;
            }

            foreach (var entry in collector.Entrys)
            {
                if (entry == null || string.IsNullOrEmpty (entry.key))
                {
                    continue;
                }

                if (!targetEntryKeys.Contains (entry.key))
                {
                    targetEntryKeys.Add (entry.key);
                }
            }
        }

        blockKeys.Sort ();

        foreach (var pair in entryKeysTable)
        {
            pair.Value.Sort ();
        }
    }

    [MenuItem ("Tools/Block/Refresh Key Table")]
    static void RefreshFromMenu ()
    {
        Refresh ();
        Debug.Log ($"BlockEditorUtility Refresh 完成, block數量: {blockKeys.Count}");
    }
}
