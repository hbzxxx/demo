using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public Image Icon;
    public Text NameText;
    public Text PriceText;
    public Text StockText;
    public Button BuyButton;

    private string itemID;

    public void Setup(ItemData data, int price, int stock)
    {
        itemID = data.ID;
        NameText.text = data.Name;
        PriceText.text = $"{price} 金币";
        StockText.text = stock > 0 ? $"库存: {stock}" : "无限";
        BuyButton.interactable = stock > 0 || InventoryManager.Instance.CurrentGold >= price;
    }

    public void OnBuyClicked()
    {
        ShopManager.Instance.BuyItem(itemID);
    }
}
