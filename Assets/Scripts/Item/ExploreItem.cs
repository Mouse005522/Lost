using UnityEngine;
using UnityEngine.EventSystems;

public class ExploreItem : Item, IPointerClickHandler
{
    void IPointerClickHandler.OnPointerClick (PointerEventData eventData)
    {
        HandleClick ();
    }

    void HandleClick ()
    {
        if (IsInteractable == false)
        {
            return;
        }

        if (string.IsNullOrEmpty (Msg))
        {
            Debug.LogError ($"{name} 沒有綁定探索字串", this);
            return;
        }

        if (TryGetItemManager (out ItemManager itemManager))
        {
            var dto = new ItemDTO (ItemMsgType.Explore, Msg);

            itemManager.ReceiveItemMessage (dto);

            this.gameObject.SetActive (false);
        }
    }
}
