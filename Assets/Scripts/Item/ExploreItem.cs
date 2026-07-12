using UnityEngine;
using UnityEngine.EventSystems;

public class ExploreItem : Item, IPointerClickHandler
{
    [SerializeField]
    GameObject entity;

    [SerializeField]
    GameObject interactTips;

    [SerializeField]
    float interactTipsDuration = 3f;

    bool isInteractTipsVisible;
    float interactTipsTimer;

    protected override void Setup ()
    {
        base.Setup ();
        SetEntityVisible (true);
        SetInteractTipsVisible (false);
        isInteractTipsVisible = false;
        interactTipsTimer = 0f;
    }

    protected override void DoUpdate (float deltaTime)
    {
        base.DoUpdate (deltaTime);

        if (isInteractTipsVisible)
        {
            interactTipsTimer -= deltaTime;

            if (interactTipsTimer <= 0f)
            {
                HideInteractTips ();
            }
        }
    }

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

        if (TryGetItemManager (out ItemManager itemManager))
        {
            if (itemManager.CanInteractItem (this))
            {
                if (string.IsNullOrEmpty (Msg))
                {
                    Debug.LogError ($"{name} 沒有綁定探索字串", this);
                }
                else
                {
                    var dto = new ItemDTO (ItemMsgType.Explore, Msg);

                    itemManager.ReceiveItemMessage (dto);

                    SetInteractable (false);
                    SetEntityVisible (false);
                    SetInteractTipsVisible (true);
                    isInteractTipsVisible = true;
                    interactTipsTimer = interactTipsDuration;
                }
            }
        }
        else
        {
            Debug.LogError ($"{name} 無法取得 ItemManager", this);
        }
    }

    void HideInteractTips ()
    {
        isInteractTipsVisible = false;
        interactTipsTimer = 0f;
        SetInteractTipsVisible (false);
    }

    void SetEntityVisible (bool isVisible)
    {
        if (entity != null)
        {
            entity.SetActive (isVisible);
        }
    }

    void SetInteractTipsVisible (bool isVisible)
    {
        if (interactTips != null)
        {
            interactTips.SetActive (isVisible);
        }
    }
}
