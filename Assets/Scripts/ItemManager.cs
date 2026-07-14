using UnityEngine;
using System.Collections.Generic;
using Kun.Tool;
using UnityEngine;

public class ItemManager : FlowManager
{
    [EasyInject]
    OverlayCanvasController overlayCanvasController;

    [EasyInject]
    PlayerController playerController;

    [EasyInject]
    InputController inputController;

    [EasyInject]
    BlockManager blockManager;

    [Header("互動範圍")]
    [SerializeField]
    float interactDistance = 0.3f;

    [Header ("傳送範圍")]
    [SerializeField]
    float portalDistance = 0.5f;

    List<Item> items = new List<Item> ();

    List<TipItem> tipItems = new List<TipItem> ();

    List<PortalCollector> currentPortalCollectors = new List<PortalCollector> ();

    public IReadOnlyList<PortalCollector> CurrentPortalCollectors => currentPortalCollectors;

    public void SetCurrentBlock(RuntimeBlock runtimeBlock)
    {
        currentPortalCollectors.Clear ();
        items.Clear();
        if (runtimeBlock != null && runtimeBlock.Items != null)
        {
            if (runtimeBlock.PortalCollectors != null)
            {
                currentPortalCollectors.AddRange (runtimeBlock.PortalCollectors);
            }

            items.AddRange(runtimeBlock.Items);
        }

        tipItems = items.FindAll(item => item is TipItem).ConvertAll(item => item as TipItem);
    }

    public void ReceiveItemMessage (ItemDTO itemDto)
    {
        if (itemDto == null)
        {
            Debug.LogError ($"{name} 收到空的 ItemDTO", this);
        }
        else if (string.IsNullOrEmpty (itemDto.msg))
        {
            Debug.LogError ($"{name} 收到空的 Item 訊息內容", this);
        }
        else
        {
            Debug.Log ($"收到 {itemDto.msgType} 訊息: {itemDto.msg}", this);

            if (itemDto.msgType == ItemMsgType.Tip)
            {
                bool end = receiveExploreMsgs.Contains (itemDto.msg);

                overlayCanvasController.ShowTip (itemDto.msg, end);

                if (end)
                {
                    receiveExploreMsgs.Remove (itemDto.msg);

                    if (tipItems.TryFind (i => i.Msg == itemDto.msg, out TipItem tip))
                    {
                        tip.SetInteractable (false);
                    }
                    else
                    {
                        Debug.LogError ($"{name} 找不到對應的 TipItem: {itemDto.msg}", this);
                    }
                }
            }
            else if (itemDto.msgType == ItemMsgType.Explore)
            {
                receiveExploreMsgs.Add (itemDto.msg);
            }
            else 
            {
                Debug.LogError ($"{name} 收到未知的 Item 訊息類型: {itemDto.msgType}", this);
            }
        }
    }

    protected override void DoPhysicsRateUpdate (float physicsRate)
    {
        base.DoPhysicsRateUpdate (physicsRate);
        CheckPortal ();
    }

    void CheckPortal ()
    {
        if (inputController == null || inputController.InteractInput == null)
        {
            return;
        }

        Vector2Int moveInput = inputController.InteractInput.GetMoveInput ();
        if (moveInput == Vector2Int.zero)
        {
            return;
        }

        if (TryGetPortalDirection (moveInput, out PortalDirection direction))
        {
            foreach (PortalCollector portalCollector in currentPortalCollectors)
            {
                if (portalCollector != null && portalCollector.Direction == direction && CanPortalPosition (portalCollector.transform.position))
                {
                    if (blockManager != null)
                    {
                        if (blockManager.TryEnterBlockCollector (portalCollector.blockKey, portalCollector.entryKey))
                        {
                            break;
                        }
                    }
                    else
                    {
                        Debug.LogError ($"{name} 尚未透過 DI 取得 BlockManager", this);
                    }
                }
            }
        }
    }

    bool TryGetPortalDirection (Vector2Int moveInput, out PortalDirection direction)
    {
        if (moveInput.y > 0)
        {
            direction = PortalDirection.Front;
            return true;
        }
        else if (moveInput.y < 0)
        {
            direction = PortalDirection.Back;
            return true;
        }
        else if (moveInput.x < 0)
        {
            direction = PortalDirection.Left;
            return true;
        }
        else if (moveInput.x > 0)
        {
            direction = PortalDirection.Right;
            return true;
        }
        else
        {
            direction = PortalDirection.Front;
            return false;
        }
    }

    public bool CanInteractItem (Item item)
    {
        if (item != null)
        {
            return CanInteractPosition (item.transform.position);
        }
        else
        {
            Debug.LogError ($"{name} 收到空的 Item", this);
            return false;
        }
    }

    public bool CanInteractPosition (Vector3 position)
    {
        if (playerController != null)
        {
            float distance = Vector3.Distance (position, playerController.transform.position);
            return distance <= interactDistance;
        }
        else
        {
            Debug.LogError ($"{name} 尚未透過 DI 取得 PlayerController", this);
            return false;
        }
    }

    public bool CanPortalPosition (Vector3 position)
    {
        if (playerController != null)
        {
            float distance = Vector3.Distance (position, playerController.transform.position);
            return distance <= portalDistance;
        }
        else
        {
            Debug.LogError ($"{name} 尚未透過 DI 取得 PlayerController", this);
            return false;
        }
    }

    List<string> receiveExploreMsgs = new List<string> ();
}
