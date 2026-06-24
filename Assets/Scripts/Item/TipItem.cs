using UnityEngine;
using UnityEngine.UI;
using Kun.Tool;

public class TipItem : Item
{
    [SerializeField]
    Button tipButton;

    [SerializeField]
    float interactDistance = 1f;

    [EasyInject]
    PlayerController playerController;

    bool isTipButtonVisible;

    protected override void Setup ()
    {
        base.Setup ();
        UpdateTipButtonState ();
    }

    protected override void DoUpdate (float deltaTime)
    {
        base.DoUpdate (deltaTime);
        UpdateTipButtonState ();
    }

    protected override void DoGameQuit ()
    {
        base.DoGameQuit ();
        UnbindButton ();
    }

    public override void SetInteractable (bool canInteract)
    {
        base.SetInteractable (canInteract);
        UpdateTipButtonState ();
    }

    void BindButton ()
    {
        if (tipButton != null)
        {
            tipButton.onClick.RemoveListener (SendTipMessage);
            tipButton.onClick.AddListener (SendTipMessage);
        }
    }

    void UnbindButton ()
    {
        if (tipButton != null)
        {
            tipButton.onClick.RemoveListener (SendTipMessage);
        }
    }

    void SendTipMessage ()
    {
        if (IsInteractable == false)
        {
            SetTipButtonVisible (false);
            return;
        }

        if (string.IsNullOrEmpty (Msg))
        {
            Debug.LogError ($"{name} 沒有綁定提示字串", this);
        }
        else
        {
            if (TryGetItemManager (out ItemManager itemManager))
            {
                var dto = new ItemDTO (ItemMsgType.Tip, Msg);

                itemManager.ReceiveItemMessage (dto);
            }
        }
    }

    void UpdateTipButtonState ()
    {
        if (IsInteractable)
        {
            if (playerController != null)
            {
                float distance = Vector3.Distance (transform.position, playerController.transform.position);

                SetTipButtonVisible (distance <= interactDistance);
            }
            else
            {
                SetTipButtonVisible (false);
            }
        }
        else
        {
            SetTipButtonVisible (false);
        }
    }

    void SetTipButtonVisible (bool isVisible)
    {
        if (tipButton == null)
        {
            return;
        }

        if (isTipButtonVisible == isVisible && tipButton.gameObject.activeSelf == isVisible)
        {
            return;
        }

        isTipButtonVisible = isVisible;
        tipButton.gameObject.SetActive (isVisible);

        if (isVisible)
        {
            BindButton ();
        }
        else
        {
            UnbindButton ();
        }
    }
}
