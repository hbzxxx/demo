using System;

[Serializable]
public class InventorySlot
{
    public string ItemID;
    public int Count;
    public ItemData ItemData;

    public bool IsEmpty => string.IsNullOrEmpty(ItemID) || Count <= 0;

    public InventorySlot()
    {
        ItemID = "";
        Count = 0;
        ItemData = null;
    }

    public InventorySlot(string itemID, int count, ItemData data)
    {
        ItemID = itemID;
        Count = count;
        ItemData = data;
    }
}
