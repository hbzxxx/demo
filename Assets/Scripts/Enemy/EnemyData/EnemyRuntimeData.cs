[System.Serializable]
public class EnemyRuntimeData
{
    [Header("当前生命值")]
    public float curHealth;
    [Header("最大生命值")]
    public float maxHealth;
    [Header("最后一次攻击时间")]
    public float lastAttackTime;
    [Header("是否死亡")]
    public bool isDead;

    public void Init(EnemyData data)
    {
        maxHealth = data.maxHealth;
        curHealth = data.curHealth;
        lastAttackTime = 0;
        isDead = false;
    }
}
