using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
public enum EnemyState
{
    Idle,
    Run,
    Attack
}
public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;//敌人数据
    [HideInInspector]
    public Animator animator;//敌人动画
    public GameObject visual;//敌人模型
    public Rigidbody2D rigidbody2D;//敌人刚体
    public Transform player;//玩家的位置
    public LayerMask playerlayerMask;//玩家图层
    private EnemyBaseState currentState;//当前敌人状态
    private Dictionary<EnemyState, EnemyBaseState> statePool;

    public bool isDie;
    private void Awake()
    {
        rigidbody2D=GetComponent<Rigidbody2D>();
        playerlayerMask = LayerMask.GetMask("Player");
        isDie = false;
        player = GameObject.Find("Player").transform;
        animator = visual.GetComponent<Animator>();
        statePool = new Dictionary<EnemyState, EnemyBaseState>()
        {
            { EnemyState.Idle, new EnemyIdleState(this) },
            { EnemyState.Run, new EnemyRunState(this) },
            { EnemyState.Attack, new EnemyAttackState(this) },
        };
    }
    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
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
