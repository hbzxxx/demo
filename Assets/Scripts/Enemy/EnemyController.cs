using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
public enum EnemyState
{
    Idle,
    Run,
    Attack,
    Dead
}
public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;//敌人数据
    [HideInInspector]
    public Animator animator;//敌人动画
    public GameObject visual;//敌人模型
    public SpriteRenderer sr;//敌人图片
    public Color originalColor;//原始颜色
    public Rigidbody2D rigidbody2D;//敌人刚体
    public Transform player;//玩家的位置
    public LayerMask playerlayerMask;//玩家图层
    private EnemyBaseState currentState;//当前敌人状态
    private Dictionary<EnemyState, EnemyBaseState> statePool;

    private void Awake()
    {
        sr = visual.GetComponent<SpriteRenderer>();
        originalColor= sr.color;
        rigidbody2D =GetComponent<Rigidbody2D>();
        playerlayerMask = LayerMask.GetMask("Player");
        player = GameObject.Find("Player").transform;
        animator = visual.GetComponent<Animator>();
        statePool = new Dictionary<EnemyState, EnemyBaseState>()
        {
            { EnemyState.Idle, new EnemyIdleState(this) },
            { EnemyState.Run, new EnemyRunState(this) },
            { EnemyState.Attack, new EnemyAttackState(this) },
            { EnemyState.Dead, new EnemyDeadState(this) },
        };
    }
    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }
    public void Hit(float damage)//敌人受伤
    {
        StartCoroutine(HitFlash(damage));
    }
    IEnumerator HitFlash(float damage)
    {
        if (enemyData.isDead)
        {
            Debug.Log("敌人死亡");
            yield break;
        }
        sr.color = new Color32(255, 85, 85, 255);
        yield return new WaitForSeconds(0.2f);
        enemyData.curHealth = Math.Max(enemyData.curHealth - damage, 0);
        Debug.Log($"受到玩家的{damage}点伤害");
        if (enemyData.curHealth == 0)
        {
            enemyData.isDead = true;
            sr.color = Color.white;
            GetComponent<CapsuleCollider2D>().enabled = false;
            Destroy(gameObject, 5f);
            SwitchState(EnemyState.Dead);
        }
        if (enemyData.isDead) yield break;
        yield break;
    }
    private void Start()
    {
        SwitchState(EnemyState.Idle);
    }
    public void SwitchState(EnemyState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState();
        }

        if (statePool.TryGetValue(newState, out EnemyBaseState state))
        {
            currentState = state;
            currentState.EnterState();
        }
        else
        {
            Debug.LogError($"未找到状态：{newState}，请检查状态池！");
        }
    }
}
