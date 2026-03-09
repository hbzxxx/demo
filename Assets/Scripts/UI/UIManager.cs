using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("任务UI")]
    public GameObject QuestPanel;
    public Transform QuestListContent;
    public GameObject QuestItemPrefab;

    [Header("背包UI")]
    public GameObject InventoryPanel;
    public Transform InventoryContent;
    public GameObject InventorySlotPrefab;
    public Text GoldText;

    [Header("商店UI")]
    public GameObject ShopPanel;
    public Transform ShopContent;
    public GameObject ShopItemPrefab;

    [Header("对话框")]
    public GameObject DialogPanel;
    public Text DialogTitleText;
    public Text DialogContentText;
    public Button DialogCloseButton;

    private void Awake()
    {
        Instance = this;
        EventCenter.AddListener(EventType.INVENTORY_UPDATED, UpdateInventoryUI);
        EventCenter.AddListener<int>(EventType.GOLD_CHANGED, OnGoldChanged);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener(EventType.INVENTORY_UPDATED, UpdateInventoryUI);
        EventCenter.RemoveListener<int>(EventType.GOLD_CHANGED, OnGoldChanged);
    }

    private void Start()
    {
        UpdateInventoryUI();
        OnGoldChanged(InventoryManager.Instance.CurrentGold);
    }

    public void ToggleQuestPanel()
    {
        QuestPanel.SetActive(!QuestPanel.activeSelf);
        if (QuestPanel.activeSelf)
        {
            UpdateQuestUI();
        }
    }

    public void ToggleInventoryPanel()
    {
        InventoryPanel.SetActive(!InventoryPanel.activeSelf);
        if (InventoryPanel.activeSelf)
        {
            UpdateInventoryUI();
        }
    }

    public void ToggleShopPanel()
    {
        ShopPanel.SetActive(!ShopPanel.activeSelf);
    }

    private void UpdateQuestUI()
    {
        foreach (Transform child in QuestListContent)
        {
            Destroy(child.gameObject);
        }

        var availableQuests = QuestManager.Instance.GetAvailableQuests();
        var acceptedQuests = QuestManager.Instance.GetAcceptedQuests();

        foreach (var quest in acceptedQuests)
        {
            CreateQuestItem(quest);
        }
        foreach (var quest in availableQuests)
        {
            CreateQuestItem(quest);
        }
    }

    private void CreateQuestItem(QuestData quest)
    {
        GameObject obj = Instantiate(QuestItemPrefab, QuestListContent);
        QuestItem item = obj.GetComponent<QuestItem>();
        item.Setup(quest);
    }

    private void UpdateInventoryUI()
    {
        foreach (Transform child in InventoryContent)
        {
            Destroy(child.gameObject);
        }

        var slots = InventoryManager.Instance.GetAllSlots();
        foreach (var slot in slots)
        {
            GameObject obj = Instantiate(InventorySlotPrefab, InventoryContent);
            InventorySlotUI slotUI = obj.GetComponent<InventorySlotUI>();
            slotUI.Setup(slot);
        }

        OnGoldChanged(InventoryManager.Instance.CurrentGold);
    }

    private void OnGoldChanged(int gold)
    {
        if (GoldText != null)
        {
            GoldText.text = gold.ToString();
        }
    }

    public void ShowDialog(string title, string content)
    {
        DialogPanel.SetActive(true);
        DialogTitleText.text = title;
        DialogContentText.text = content;
    }

    public void CloseDialog()
    {
        DialogPanel.SetActive(false);
    }

    public void UpdateShopUI(ShopData shop)
    {
        foreach (Transform child in ShopContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in shop.Items)
        {
            GameObject obj = Instantiate(ShopItemPrefab, ShopContent);
            ShopItemUI itemUI = obj.GetComponent<ShopItemUI>();
            ItemData data = InventoryManager.Instance.GetItemData(item.ItemID);
            itemUI.Setup(data, item.Price, item.Stock);
        }
    }
}
