using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    public GameObject prefab;          // 玩家对应的预制体
    public string playerName;          // 玩家名称
    public float moveSpeed;            // 移动速度
    public int maxHealth;              // 最大生命值
    public int curHealth;              // 当前生命值
    public float invincibilityTime;    // 无敌时间
    public bool invincibility;         // 是否在无敌状态
    public bool isDead;                // 是否死亡

    public PlayerData Clone()
    {
        PlayerData newData = ScriptableObject.CreateInstance<PlayerData>();

        newData.prefab = this.prefab;
        newData.playerName = this.playerName;
        newData.moveSpeed = this.moveSpeed;
        newData.maxHealth = this.maxHealth;
        newData.invincibilityTime = this.invincibilityTime;

        newData.curHealth = this.maxHealth;          // 初始血量
        newData.invincibility = this.invincibility;  // 初始无无敌状态
        newData.isDead = this.isDead;                // 初始未死亡

        return newData;
    }
}