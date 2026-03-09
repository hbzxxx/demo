using System;

[Serializable]
public class ItemData
{
    public string ID;
    public string Name;
    public string Description;
    public ItemType Type;
    public int Price;
    public int MaxStack;
    public string Icon;
    public int Value;
}

public enum ItemType
{
    Weapon,
    Armor,
    Consumable,
    Material,
    Quest,
}
