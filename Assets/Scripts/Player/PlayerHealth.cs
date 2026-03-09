using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float MaxHealth = 100f;
    public float CurrentHealth;

    public float CurrentHealthRatio => MaxHealth > 0 ? CurrentHealth / MaxHealth : 0;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        
        EventCenter.Broadcast<float>(EventType.PLAYER_HEALTH_CHANGED, CurrentHealthRatio);
        
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        
        EventCenter.Broadcast<float>(EventType.PLAYER_HEALTH_CHANGED, CurrentHealthRatio);
    }

    private void Die()
    {
        Debug.Log("玩家死亡！");
        EventCenter.Broadcast(EventType.PLAYER_DIED);
    }
}
