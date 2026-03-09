using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Slider HealthSlider;
    public Image FillImage;
    public Color FullHealthColor = Color.green;
    public Color LowHealthColor = Color.red;

    private void Start()
    {
        EventCenter.AddListener<float>(EventType.PLAYER_HEALTH_CHANGED, OnHealthChanged);
        UpdateHealthUI(1f);
    }

    private void OnDestroy()
    {
        EventCenter.RemoveListener<float>(EventType.PLAYER_HEALTH_CHANGED, OnHealthChanged);
    }

    private void OnHealthChanged(float healthRatio)
    {
        UpdateHealthUI(healthRatio);
    }

    private void UpdateHealthUI(float healthRatio)
    {
        if (HealthSlider != null)
        {
            HealthSlider.value = healthRatio;
        }

        if (FillImage != null)
        {
            FillImage.color = Color.Lerp(LowHealthColor, FullHealthColor, healthRatio);
        }
    }
}
