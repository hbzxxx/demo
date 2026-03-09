using System;

[Serializable]
public class PlayerData
{
    public string ID;
    public string Name;
    public float MaxHealth;
    public float MoveSpeed;
    public float AttackDamage;
    public float AttackSpeed;
    public float Defense;
}

[Serializable]
public class EnemyData
{
    public string ID;
    public string Name;
    public float MaxHealth;
    public float MoveSpeed;
    public float ChaseSpeed;
    public float DetectionRadius;
    public float AttackRange;
    public int Damage;
    public float AttackCooldown;
    public float PatrolWaitTime;
}

[Serializable]
public class CharacterDataSheet
{
    public PlayerData[] Players;
    public EnemyData[] Enemies;
}
