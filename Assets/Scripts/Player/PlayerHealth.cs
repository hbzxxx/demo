using UnityEngine;

/// <summary>
/// 玩家生命值管理类
/// 负责管理玩家的生命值、伤害计算、治疗和死亡逻辑
/// 生命值数据从Excel表格中读取(通过PlayerData配置)
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// 最大生命值，从表格数据中读取
    /// </summary>
    public float MaxHealth = 100f;
    
    /// <summary>
    /// 当前生命值
    /// </summary>
    public float CurrentHealth;

    /// <summary>
    /// 当前生命值比例（0-1），用于UI显示
    /// </summary>
    public float CurrentHealthRatio => MaxHealth > 0 ? CurrentHealth / MaxHealth : 0;

    /// <summary>
    /// 玩家数据ID，用于从表格读取数据
    /// </summary>
    public string PlayerDataID = "P001";

    private void Start()
    {
        // 从表格加载玩家数据
        LoadPlayerData();
        
        // 初始化当前生命值
        CurrentHealth = MaxHealth;
    }

    /// <summary>
    /// 从表格数据中加载玩家属性
    /// </summary>
    private void LoadPlayerData()
    {
        if (DataManager.Instance != null)
        {
            PlayerData data = DataManager.Instance.GetPlayerData(PlayerDataID);
            if (data != null)
            {
                MaxHealth = data.MaxHealth;
                Debug.Log($"已从表格加载玩家数据: {data.Name}, 生命值: {MaxHealth}");
            }
        }
    }

    /// <summary>
    /// 受到伤害
    /// </summary>
    /// <param name="damage">伤害值</param>
    public void TakeDamage(float damage)
    {
        // 减少生命值
        CurrentHealth -= damage;
        
        // 生命值不能小于0
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        
        // 发送血量变化事件，通知UI更新
        EventCenter.TriggerEvent<float>(EventType.PLAYER_HEALTH_CHANGED, CurrentHealthRatio);
        
        // 检查是否死亡
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    /// <summary>
    /// 治疗
    /// </summary>
    /// <param name="amount">治疗量</param>
    public void Heal(float amount)
    {
        // 增加生命值
        CurrentHealth += amount;
        
        // 生命值不能超过最大值
        if (CurrentHealth > MaxHealth)
        {
            CurrentHealth = MaxHealth;
        }
        
        // 发送血量变化事件
        EventCenter.TriggerEvent<float>(EventType.PLAYER_HEALTH_CHANGED, CurrentHealthRatio);
    }

    /// <summary>
    /// 玩家死亡处理
    /// </summary>
    private void Die()
    {
        Debug.Log("玩家死亡！");
        // 发送玩家死亡事件
        EventCenter.TriggerEvent(EventType.PLAYER_DIED);
    }
}
