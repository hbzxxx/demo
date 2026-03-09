using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敌人控制器
/// 负责管理敌人的行为状态、属性配置和游戏逻辑
/// 所有属性从Excel表格中读取(通过EnemyData配置)
/// </summary>
public class EnemyController : MonoBehaviour
{
    #region 移动设置
    [Header("移动设置")]
    /// <summary>
    /// 巡逻移动速度
    /// </summary>
    public float MoveSpeed = 2f;
    
    /// <summary>
    /// 追击玩家时的速度
    /// </summary>
    public float ChaseSpeed = 3f;
    
    /// <summary>
    /// 敌人的Rigidbody2D组件
    /// </summary>
    public Rigidbody2D Rigidbody;
    
    /// <summary>
    /// 敌人的视觉模型对象
    /// </summary>
    public GameObject Visual;
    
    /// <summary>
    /// 敌人的Animator组件
    /// </summary>
    [HideInInspector]
    public Animator Animator;
    #endregion

    #region 感知设置
    [Header("感知设置")]
    /// <summary>
    /// 感知玩家的范围半径
    /// </summary>
    public float DetectionRadius = 5f;
    
    /// <summary>
    /// 攻击玩家的范围半径
    /// </summary>
    public float AttackRange = 1.5f;
    
    /// <summary>
    /// 玩家所在的图层
    /// </summary>
    public LayerMask PlayerLayer;
    #endregion

    #region 巡逻设置
    [Header("巡逻设置")]
    /// <summary>
    /// 巡逻点数组
    /// </summary>
    public Transform[] PatrolPoints;
    
    /// <summary>
    /// 巡逻等待时间（秒）
    /// </summary>
    public float PatrolWaitTime = 2f;
    #endregion

    #region 攻击设置
    [Header("攻击设置")]
    /// <summary>
    /// 攻击冷却时间（秒）
    /// </summary>
    public float AttackCooldown = 2f;
    
    /// <summary>
    /// 每次攻击的伤害值
    /// </summary>
    public int Damage = 10;
    #endregion

    #region 私有变量
    /// <summary>
    /// 玩家目标Transform
    /// </summary>
    private Transform playerTarget;
    
    /// <summary>
    /// 当前状态
    /// </summary>
    private EnemyBaseState currentState;
    
    /// <summary>
    /// 状态对象池
    /// </summary>
    private Dictionary<EnemyState, EnemyBaseState> statePool;
    
    /// <summary>
    /// 当前巡逻点索引
    /// </summary>
    private int currentPatrolIndex;
    
    /// <summary>
    /// 上次攻击时间
    /// </summary>
    private float lastAttackTime;
    
    /// <summary>
    /// 敌人数据ID，用于从表格读取数据
    /// </summary>
    public string EnemyDataID = "E001";
    #endregion

    private void Awake()
    {
        // 获取Animator组件
        Animator = Visual.GetComponent<Animator>();
        
        // 初始化状态池
        statePool = new Dictionary<EnemyState, EnemyBaseState>()
        {
            { EnemyState.Idle, new EnemyIdleState(this) },
            { EnemyState.Patrol, new EnemyPatrolState(this) },
            { EnemyState.Chase, new EnemyChaseState(this) },
            { EnemyState.Attack, new EnemyAttackState(this) },
        };
    }

    private void Start()
    {
        // 从表格加载敌人数据
        LoadEnemyData();
        
        // 查找玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        
        // 初始状态为待机
        SwitchState(EnemyState.Idle);
    }

    /// <summary>
    /// 从表格数据中加载敌人属性
    /// </summary>
    private void LoadEnemyData()
    {
        if (DataManager.Instance != null)
        {
            EnemyData data = DataManager.Instance.GetEnemyData(EnemyDataID);
            if (data != null)
            {
                MoveSpeed = data.MoveSpeed;
                ChaseSpeed = data.ChaseSpeed;
                DetectionRadius = data.DetectionRadius;
                AttackRange = data.AttackRange;
                Damage = data.Damage;
                AttackCooldown = data.AttackCooldown;
                PatrolWaitTime = data.PatrolWaitTime;
                Debug.Log($"已从表格加载敌人数据: {data.Name}, 生命值: {data.MaxHealth}, 伤害: {Damage}");
            }
        }
    }

    private void Update()
    {
        // 每帧更新当前状态
        if (currentState != null)
        {
            currentState.UpdateState();
        }
    }

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newState">新状态</param>
    public void SwitchState(EnemyState newState)
    {
        // 退出当前状态
        if (currentState != null)
        {
            currentState.ExitState();
        }

        // 切换到新状态
        if (statePool.TryGetValue(newState, out EnemyBaseState state))
        {
            currentState = state;
            currentState.EnterState();
        }
        else
        {
            Debug.LogError($"未找到状态：{newState}，请检查状态枚举！");
        }
    }

    /// <summary>
    /// 检测玩家是否在感知范围内
    /// </summary>
    /// <returns>是否在感知范围内</returns>
    public bool IsPlayerInDetectionRange()
    {
        if (playerTarget == null) return false;
        float distance = Vector2.Distance(transform.position, playerTarget.position);
        return distance <= DetectionRadius;
    }

    /// <summary>
    /// 检测玩家是否在攻击范围内
    /// </summary>
    /// <returns>是否在攻击范围内</returns>
    public bool IsPlayerInAttackRange()
    {
        if (playerTarget == null) return false;
        float distance = Vector2.Distance(transform.position, playerTarget.position);
        return distance <= AttackRange;
    }

    /// <summary>
    /// 获取玩家目标
    /// </summary>
    /// <returns>玩家Transform</returns>
    public Transform GetPlayerTarget()
    {
        return playerTarget;
    }

    /// <summary>
    /// 朝目标位置移动
    /// </summary>
    /// <param name="targetPosition">目标位置</param>
    /// <param name="speed">移动速度</param>
    public void MoveTowardsTarget(Vector2 targetPosition, float speed)
    {
        // 计算方向向量
        Vector2 direction = (targetPosition - Rigidbody.position).normalized;
        
        // 移动
        Rigidbody.MovePosition(Rigidbody.position + direction * speed * Time.fixedDeltaTime);
        
        // 根据移动方向翻转模型
        if (direction.x > 0)
        {
            Visual.transform.localScale = new Vector3(Mathf.Abs(Visual.transform.localScale.x), Visual.transform.localScale.y, Visual.transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            Visual.transform.localScale = new Vector3(-Mathf.Abs(Visual.transform.localScale.x), Visual.transform.localScale.y, Visual.transform.localScale.z);
        }
    }

    /// <summary>
    /// 获取下一个巡逻点位置
    /// </summary>
    /// <returns>巡逻点位置</returns>
    public Vector2 GetNextPatrolPoint()
    {
        if (PatrolPoints == null || PatrolPoints.Length == 0)
        {
            return Rigidbody.position;
        }
        return PatrolPoints[currentPatrolIndex].position;
    }

    /// <summary>
    /// 移动到下一个巡逻点
    /// </summary>
    public void MoveToNextPatrolPoint()
    {
        if (PatrolPoints == null || PatrolPoints.Length == 0) return;
        
        Vector2 targetPos = PatrolPoints[currentPatrolIndex].position;
        MoveTowardsTarget(targetPos, MoveSpeed);

        // 到达巡逻点后切换到下一个
        if (Vector2.Distance(Rigidbody.position, targetPos) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % PatrolPoints.Length;
        }
    }

    /// <summary>
    /// 是否可以攻击（冷却时间检查）
    /// </summary>
    /// <returns>是否可以攻击</returns>
    public bool CanAttack()
    {
        return Time.time - lastAttackTime >= AttackCooldown;
    }

    /// <summary>
    /// 执行攻击
    /// </summary>
    public void PerformAttack()
    {
        // 记录攻击时间
        lastAttackTime = Time.time;
        
        if (playerTarget != null)
        {
            // 获取玩家血量组件
            PlayerHealth playerHealth = playerTarget.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // 对玩家造成伤害
                playerHealth.TakeDamage(Damage);
                Debug.Log($"敌人对玩家造成 {Damage} 点伤害！");
            }
        }
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    public void StopMovement()
    {
        Rigidbody.velocity = Vector2.zero;
    }

    /// <summary>
    /// 在编辑器中绘制感知和攻击范围（仅选中时显示）
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // 绘制感知范围（黄色）
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
        
        // 绘制攻击范围（红色）
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
