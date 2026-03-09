using System;

[Serializable]
public class ShopData
{
    public string ID;
    public string Name;
    public ShopItem[] Items;
}

[Serializable]
public class ShopItem
{
    public string ItemID;
    public int Price;
    public int Stock;
    public bool IsUnlimited;
}
