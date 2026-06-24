using Kun.Tool;
using UnityEngine;

public abstract class Item : FlowComponent
{
    [ReadOnly]
    [SerializeField]
    bool isInteractable = true;

    [EasyInject]
    ItemManager itemManager;

    public bool IsInteractable => isInteractable;

    protected ItemManager ItemManager => itemManager;

    public string Msg => msg;

    [SerializeField]
    string msg;

    protected override void Setup ()
    {
        base.Setup ();
        SetInteractable (isInteractable);
    }

    public virtual void SetInteractable (bool canInteract)
    {
        isInteractable = canInteract;
    }

    protected bool TryGetItemManager (out ItemManager manager)
    {
        if (itemManager != null)
        {
            manager = itemManager;
            return true;
        }
        else
        {
            Debug.LogError ($"{name} 尚未透過 DI 取得 ItemManager", this);
            manager = null;
            return false;
        }
    }

}
