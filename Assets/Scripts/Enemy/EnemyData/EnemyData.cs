using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 在Unity编辑器中创建该资源的菜单（右键Create -> Game -> Enemy Data）
[CreateAssetMenu(fileName = "New Enemy Data", menuName = "Game/Enemy Data")]
public class EnemyData : ScriptableObject
{

    [Header("敌人固定属性")]
    public GameObject prefab;// 敌人对应的预制体

    public string enemyName;// 敌人名称
    public int attackDamage;// 攻击伤害
    public float moveSpeed;// 移动速度
    public float attackCD;// 攻击CD
    public float attackRange;// 攻击范围
    public float detectRange;// 追踪范围
    public float patrolRange;// 巡逻范围
    public int goldReward;// 击败后奖励的金币

    [Header("敌人游戏变化属性")]

    public float attackTime;// 最后一次攻击时间攻击时间
    public float maxHealth;// 最大生命值
    public float curHealth;// 当前生命值
    public bool isDead;// 是否死亡

}