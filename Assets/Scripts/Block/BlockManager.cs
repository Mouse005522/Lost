using Kun.Tool;
using UnityEngine;
using System.Collections.Generic;

public class BlockManager : FlowManager
{
    [EasyInject]
    ItemManager itemManager;

    RuntimeBlockEntry[] runtimeBlockEntries;

    BlockCollector curBlockCollector;
    BlockEntry curBlock;

    protected override void Setup ()
    {
        base.Setup ();
        var collectors = GetComponentsInChildren<BlockCollector>(true);
        runtimeBlockEntries = new RuntimeBlockEntry[collectors.Length];
        for (int i = 0; i < collectors.Length; i++)
        {
            var items = collectors[i].GetComponentsInChildren<Item>(true);
            runtimeBlockEntries[i] = new RuntimeBlockEntry(collectors[i], items);
        }
    }

    protected override void PrepareSubFlowables ()
    {
        base.PrepareSubFlowables ();
        if (runtimeBlockEntries == null)
        {
            var collectors = GetComponentsInChildren<BlockCollector> (true);
            runtimeBlockEntries = new RuntimeBlockEntry[collectors.Length];
            for (int i = 0; i < collectors.Length; i++)
            {
                var items = collectors[i].GetComponentsInChildren<Item> (true);
                runtimeBlockEntries[i] = new RuntimeBlockEntry (collectors[i], items);
            }
        }

        if (runtimeBlockEntries != null)
        {
            List<Item> allItems = new List<Item> ();
            foreach (var entry in runtimeBlockEntries)
            {
                allItems.AddRange (entry.Items);
            }
            subFlowables.AddRange (allItems.ConvertAll (item => item as IFlowable));
        }
    }

    protected override void Init ()
    {
        base.Init ();

        if (!CheckCompleteness ())
        {
            Debug.LogError ("BlockManager 完備檢查失敗");
            return;
        }

        if (TryEnterFirstBlockCollector ())
        {
            // Enter first block collector successfully
        }
        else
        {
            Debug.LogError ("BlockManager 初始化失敗, 找不到起始的 BlockCollector");
        }
    }

    bool CheckCompleteness ()
    {
        if (runtimeBlockEntries == null)
            return false;

        bool hasRoot = false;
        System.Collections.Generic.HashSet<string> keys = new System.Collections.Generic.HashSet<string> ();

        foreach (var entry in runtimeBlockEntries)
        {
            var collector = entry.Collector;
            if (string.IsNullOrEmpty (collector.key))
            {
                Debug.LogError ($"BlockCollector {collector.name} 缺少 key");
                return false;
            }

            if (!keys.Add (collector.key))
            {
                Debug.LogError ($"BlockCollector key 重複: {collector.key}");
                return false;
            }

            if (collector.isRoot)
            {
                if (hasRoot)
                {
                    Debug.LogError ("有多個為 root 的 BlockCollector");
                    return false;
                }
                hasRoot = true;

                if (string.IsNullOrEmpty (collector.rootKey))
                {
                    Debug.LogError ($"起始 BlockCollector {collector.key} 缺少 rootKey");
                    return false;
                }
            }

            if (collector.edge == null)
            {
                Debug.LogError ($"BlockCollector {collector.key} 缺少 edge 碰撞體");
                return false;
            }
        }

        if (hasRoot == false)
        {
            Debug.LogError ("找不到為 root 的 BlockCollector");
            return false;
        }

        return true;
    }

    public bool TryEnterBlockCollector (string key)
    {
        foreach (var entry in runtimeBlockEntries)
        {
            var collector = entry.Collector;
            if (collector.key == key)
            {
                if (collector.TryEnterBlock (collector.rootKey, out BlockEntry block))
                {
                    curBlockCollector = collector;
                    curBlock = block;
                    itemManager?.SetCurrentItems(entry.Items);
                    return true;
                }
                else
                {
                    Debug.LogError ($"BlockCollector key {key} 找不到起始 block : {collector.rootKey}");
                    return false;
                }
            }
        }

        Debug.LogError ($"找不到對應的 BlockCollector, key: {key}");
        return false;
    }

    bool TryEnterFirstBlockCollector ()
    {
        foreach (var entry in runtimeBlockEntries)
        {
            var collector = entry.Collector;
            if (collector.isRoot)
            {
                if (collector.TryEnterBlock (collector.rootKey, out BlockEntry block))
                {
                    curBlockCollector = collector;
                    curBlock = block;
                    itemManager?.SetCurrentItems(entry.Items);
                    return true;
                }
                else
                {
                    Debug.LogError ($"起始 BlockCollector 找不到起始 block : {collector.rootKey}");
                    return false;
                }
            }
        }

        Debug.LogError ("找不到為 root 的 BlockCollector");
        return false;
    }

    /// <summary>
    /// 檢查並限制移動者在區塊邊界內。
    /// 邏輯為：檢查移動後的預期位置是否出界，
    /// 原則上僅限制超過邊界方向的位移。
    /// </summary>
    /// <param name="origin">移動前的原點</param>
    /// <param name="moveDelta">移動向量</param>
    /// <param name="moverEdge">移動者本身的碰撞體</param>
    /// <returns>修正後的實際座標</returns>
    public Vector3 CheckEdge (Vector3 origin, Vector3 moveDelta, BoxCollider2D moverEdge)
    {
        // 若缺乏邊界或移動者沒有給予碰撞體，則不作限制，直接套用移動向量
        if (curBlockCollector == null || curBlockCollector.edge == null || moverEdge == null)
            return origin + moveDelta;

        // 取得當前區塊的邊界範圍
        Bounds blockBounds = curBlockCollector.edge.bounds;

        // 取得移動者當前的碰撞邊界
        Bounds currentMoverBounds = moverEdge.bounds;
        Vector3 extents = currentMoverBounds.extents;

        Vector3 allowedDelta = moveDelta;

        // === X軸向檢查 ===
        if (allowedDelta.x > 0) // 往右移
        {
            float distToEdge = blockBounds.max.x - (currentMoverBounds.center.x + extents.x);
            // 若已經貼牆或微小穿透，該方向不給走，但「不反推」 (距離不可小於0)
            if (distToEdge < 0) distToEdge = 0;

            if (allowedDelta.x > distToEdge)
                allowedDelta.x = distToEdge;
        }
        else if (allowedDelta.x < 0) // 往左移
        {
            float distToEdge = blockBounds.min.x - (currentMoverBounds.center.x - extents.x);
            // 若已經貼牆或微小穿透，不反推 (距離不可大於0)
            if (distToEdge > 0) distToEdge = 0;

            if (allowedDelta.x < distToEdge)
                allowedDelta.x = distToEdge;
        }

        // === Y軸向檢查 ===
        if (allowedDelta.y > 0) // 往上移
        {
            float distToEdge = blockBounds.max.y - (currentMoverBounds.center.y + extents.y);
            if (distToEdge < 0) distToEdge = 0;

            if (allowedDelta.y > distToEdge)
                allowedDelta.y = distToEdge;
        }
        else if (allowedDelta.y < 0) // 往下移
        {
            float distToEdge = blockBounds.min.y - (currentMoverBounds.center.y - extents.y);
            if (distToEdge > 0) distToEdge = 0;

            if (allowedDelta.y < distToEdge)
                allowedDelta.y = distToEdge;
        }

        // 套用最終允許的位移量，確保原本不動的軸或者未出界的軸完美保留
        return origin + allowedDelta;
    }
}
