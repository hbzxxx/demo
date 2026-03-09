using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance;

    private Dictionary<string, ShopData> shopDataDict = new Dictionary<string, ShopData>();
    private ShopData currentShop;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadShopData(ShopData[] shops)
    {
        shopDataDict.Clear();
        foreach (var shop in shops)
        {
            shopDataDict[shop.ID] = shop;
        }
        Debug.Log($"加载了 {shopDataDict.Count} 个商店数据");
    }

    public void OpenShop(string shopID)
    {
        if (shopDataDict.TryGetValue(shopID, out ShopData shop))
        {
            currentShop = shop;
            EventCenter.Broadcast<ShopData>(EventType.SHOP_OPENED, shop);
            Debug.Log($"打开商店: {shop.Name}");
        }
    }

    public bool BuyItem(string itemID)
    {
        if (currentShop == null) return false;

        ShopItem shopItem = null;
        foreach (var item in currentShop.Items)
        {
            if (item.ItemID == itemID)
            {
                shopItem = item;
                break;
            }
        }

        if (shopItem == null)
        {
            Debug.LogWarning("商店中没有此物品");
            return false;
        }

        if (!shopItem.IsUnlimited && shopItem.Stock <= 0)
        {
            Debug.LogWarning("物品已售罄");
            return false;
        }

        if (!InventoryManager.Instance.SpendGold(shopItem.Price))
        {
            Debug.LogWarning("金币不足");
            return false;
        }

        InventoryManager.Instance.AddItem(itemID, 1);

        if (!shopItem.IsUnlimited)
        {
            shopItem.Stock--;
        }

        EventCenter.Broadcast<string>(EventType.ITEM_PURCHASED, itemID);
        Debug.Log($"购买物品: {itemID}, 价格: {shopItem.Price}");
        return true;
    }

    public ShopData GetCurrentShop()
    {
        return currentShop;
    }

    public ShopData GetShop(string shopID)
    {
        return shopDataDict.TryGetValue(shopID, out ShopData shop) ? shop : null;
    }
}
