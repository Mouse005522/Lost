using UnityEngine;
using System.Collections.Generic;
using Kun.Tool;

public class ItemManager : FlowManager
{
    [EasyInject]
    OverlayCanvasController overlayCanvasController;

    [EasyInject]
    PlayerController playerController;

    [Header("互動範圍")]
    [SerializeField]
    float interactDistance = 1f;

    List<Item> items = new List<Item> ();

    List<TipItem> tipItems = new List<TipItem> ();

    public void SetCurrentItems(IEnumerable<Item> newItems)
    {
        items.Clear();
        if (newItems != null)
        {
            items.AddRange(newItems);
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

    List<string> receiveExploreMsgs = new List<string> ();
}
