using System.Collections.Generic;
using System.Collections.Generic;
using Kun.Tool;
using UnityEngine;
using UnityEngine.UI;

public class OverlayCanvasController : FlowManager
{
    [SerializeField]
    GameObject bg;

    [SerializeField]
    Button closeButton;

    Dictionary<(string tipKey, bool isEnd), GameObject> tipTable = new Dictionary<(string tipKey, bool isEnd), GameObject> ();

    GameObject lastOpenTipGameObject;

    protected override void Init ()
    {
        base.Init ();
        CacheTipTable ();
        BindCloseButton ();
        CloseTip ();
    }

    internal void ShowTip (string msg, bool isEnd)
    {
        if (string.IsNullOrEmpty (msg))
        {
            Debug.LogError ("tip key 為空", this);
        }
        else if (tipTable.TryGetValue ((msg, isEnd), out GameObject tipGameObject))
        {
            if (tipGameObject != null)
            {
                CloseTip ();

                SetBgActive (true);

                tipGameObject.SetActive (true);
                lastOpenTipGameObject = tipGameObject;
            }
            else
            {
                Debug.LogError ($"tip key: {msg}, isEnd: {isEnd} 對應的 GameObject 為空", this);
            }
        }
        else
        {
            Debug.LogError ($"找不到 tip key: {msg}, isEnd: {isEnd}", this);
        }
    }

    internal void CloseTip ()
    {
        SetBgActive (false);

        foreach (var pair in tipTable)
        {
            if (pair.Value != null)
            {
                pair.Value.SetActive (false);
            }
        }

        lastOpenTipGameObject = null;
    }

    void SetBgActive (bool isActive)
    {
        if (bg != null)
        {
            bg.SetActive (isActive);
        }
    }

    void BindCloseButton ()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener (CloseTip);
            closeButton.onClick.AddListener (CloseTip);
        }
    }

    void UnbindCloseButton ()
    {
        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener (CloseTip);
        }
    }

    void CacheTipTable ()
    {
        tipTable.Clear ();

        TipUICollector[] tipCollectors = GetComponentsInChildren<TipUICollector> (true);

        foreach (TipUICollector tipCollector in tipCollectors)
        {
            if (string.IsNullOrEmpty (tipCollector.Key))
            {
                Debug.LogError ($"{tipCollector.name} 沒有綁定 tip key", tipCollector);
            }
            else if (tipTable.ContainsKey ((tipCollector.Key, tipCollector.IsEnd)) == false)
            {
                tipTable.Add ((tipCollector.Key, tipCollector.IsEnd), tipCollector.gameObject);
            }
            else
            {
                Debug.LogError ($"重複的 tip key: {tipCollector.Key}, isEnd: {tipCollector.IsEnd}", tipCollector);
            }
        }
    }

    protected override void DoGameQuit ()
    {
        base.DoGameQuit ();
        UnbindCloseButton ();
    }
}
