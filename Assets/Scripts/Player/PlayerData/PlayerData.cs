using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Game/Player Data")]
public class PlayerData : ScriptableObject
{
    public GameObject prefab;// 玩家对应的预制体
    public string playerName;// 玩家名称
    public float moveSpeed;// 移动速度
    public int maxHealth;// 最大生命值
    public int curHealth;// 当前生命值
    public float invincibilityTime;// 无敌时间
    public bool invincibility;// 是否在无敌状态
    public bool isDead;// 是否死亡
}