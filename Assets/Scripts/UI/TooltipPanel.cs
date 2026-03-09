using UnityEngine;

public class TooltipPanel : MonoBehaviour
{
    public GameObject Panel;
    public Text TitleText;
    public Text DescText;
    public Text TypeText;
    public Text PriceText;

    public void Show(ItemData data)
    {
        Panel.SetActive(true);
        TitleText.text = data.Name;
        DescText.text = data.Description;
        TypeText.text = data.Type.ToString();
        PriceText.text = data.Price > 0 ? $"{data.Price} 金币" : "";
    }

    public void Hide()
    {
        Panel.SetActive(false);
    }
}
