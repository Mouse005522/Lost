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
    Explore,
    Tip,
}
