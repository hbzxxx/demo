using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public Image Icon;
    public Text CountText;
    public Text NameText;
    public TooltipPanel Tooltip;

    public void Setup(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            Icon.gameObject.SetActive(false);
            CountText.text = "";
            NameText.text = "";
            return;
        }

        Icon.gameObject.SetActive(true);
        CountText.text = slot.Count > 1 ? slot.Count.ToString() : "";
        NameText.text = slot.ItemData?.Name ?? "";
    }
}
