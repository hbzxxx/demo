using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public int MaxSlots = 20;
    public int CurrentGold = 0;

    private List<InventorySlot> slots = new List<InventorySlot>();
    private Dictionary<string, ItemData> itemDataDict = new Dictionary<string, ItemData>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < MaxSlots; i++)
        {
            slots.Add(new InventorySlot());
        }
    }

    public void LoadItemData(ItemData[] items)
    {
        itemDataDict.Clear();
        foreach (var item in items)
        {
            itemDataDict[item.ID] = item;
        }
        Debug.Log($"加载了 {itemDataDict.Count} 个物品数据");
    }

    public void AddGold(int amount)
    {
        CurrentGold += amount;
        EventCenter.Broadcast<int>(EventType.GOLD_CHANGED, CurrentGold);
    }

    public bool SpendGold(int amount)
    {
        if (CurrentGold >= amount)
        {
            CurrentGold -= amount;
            EventCenter.Broadcast<int>(EventType.GOLD_CHANGED, CurrentGold);
            return true;
        }
        return false;
    }

    public bool AddItem(string itemID, int count = 1)
    {
        if (!itemDataDict.TryGetValue(itemID, out ItemData data))
        {
            Debug.LogWarning($"物品ID不存在: {itemID}");
            return false;
        }

        if (data.MaxStack > 1)
        {
            foreach (var slot in slots)
            {
                if (!slot.IsEmpty && slot.ItemID == itemID && slot.Count < data.MaxStack)
                {
                    int space = data.MaxStack - slot.Count;
                    int toAdd = Mathf.Min(space, count);
                    slot.Count += toAdd;
                    count -= toAdd;
                    if (count <= 0)
                    {
                        EventCenter.Broadcast(EventType.INVENTORY_UPDATED);
                        return true;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].IsEmpty)
            {
                slots[i] = new InventorySlot(itemID, count, data);
                EventCenter.Broadcast(EventType.INVENTORY_UPDATED);
                return true;
            }
        }

        Debug.LogWarning("背包已满");
        return false;
    }

    public bool RemoveItem(string itemID, int count = 1)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (!slots[i].IsEmpty && slots[i].ItemID == itemID)
            {
                if (slots[i].Count >= count)
                {
                    slots[i].Count -= count;
                    if (slots[i].Count <= 0)
                    {
                        slots[i] = new InventorySlot();
                    }
                    EventCenter.Broadcast(EventType.INVENTORY_UPDATED);
                    return true;
                }
                else
                {
                    count -= slots[i].Count;
                    slots[i] = new InventorySlot();
                }
            }
        }
        return false;
    }

    public int GetItemCount(string itemID)
    {
        int count = 0;
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.ItemID == itemID)
            {
                count += slot.Count;
            }
        }
        return count;
    }

    public List<InventorySlot> GetAllSlots()
    {
        return slots;
    }

    public ItemData GetItemData(string itemID)
    {
        return itemDataDict.TryGetValue(itemID, out ItemData data) ? data : null;
    }
}
