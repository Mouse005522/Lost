using UnityEngine;
using System.Collections.Generic;

public class BlockCollector : MonoBehaviour
{
    [SerializeField]
    public string key;

    [SerializeField]
    public bool isRoot;

    [SerializeField]
    public string rootKey;

    [SerializeField]
    Transform camPoint;

    public Transform CamPoint => camPoint;

    [Header ("進入點")]
    [SerializeField]
    List<BlockEntry> entrys = new List<BlockEntry> ();

    public IReadOnlyList<BlockEntry> Entrys => entrys;

    [SerializeField]
    public BoxCollider2D edge;

    public bool TryEnterBlock (string targetKey, out BlockEntry block)
    {
        foreach (var entry in entrys)
        {
            if (entry.key == targetKey)
            {
                block = entry;
                return true;
            }
        }

        block = null;
        return false;
    }
}