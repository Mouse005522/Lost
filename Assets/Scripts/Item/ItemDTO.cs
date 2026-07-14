using System;

[Serializable]
public class ItemDTO
{
    public ItemMsgType msgType;
    public string msg;

    public ItemDTO (ItemMsgType messageType, string msg)
    {
        msgType = messageType;
        this.msg = msg;
    }
}

public enum ItemMsgType
{
    /// <summary>
    /// 撿起物件時顯示
    /// </summary>
    Explore,
    /// <summary>
    /// NPC提示訊息
    /// </summary>
    Tip,
}
