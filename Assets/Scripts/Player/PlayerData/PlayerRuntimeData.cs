using UnityEngine;

[System.Serializable]
public class PlayerRuntimeData
{
    [Header("当前生命值")]
    public int curHealth;
    [Header("最大生命值")]
    public int maxHealth;
    [Header("是否无敌")]
    public bool isInvincible;
    [Header("无敌结束时间")]
    public float invincibilityEndTime;
    [Header("是否死亡")]
    public bool isDead;

    public void Init(PlayerData data)
    {
        maxHealth = data.maxHealth;
        curHealth = data.curHealth;
        isInvincible = false;
        invincibilityEndTime = 0;
        isDead = false;
    }
}
