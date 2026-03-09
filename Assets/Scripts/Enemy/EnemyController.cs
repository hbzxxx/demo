using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("移动设置")]
    public float MoveSpeed = 2f;
    public float ChaseSpeed = 3f;
    public Rigidbody2D Rigidbody;
    public GameObject Visual;
    [HideInInspector]
    public Animator Animator;

    [Header("感知设置")]
    public float DetectionRadius = 5f;
    public float AttackRange = 1.5f;
    public LayerMask PlayerLayer;

    [Header("巡逻设置")]
    public Transform[] PatrolPoints;
    public float PatrolWaitTime = 2f;

    [Header("攻击设置")]
    public float AttackCooldown = 2f;
    public int Damage = 10;

    private Transform playerTarget;
    private EnemyBaseState currentState;
    private Dictionary<EnemyState, EnemyBaseState> statePool;

    private int currentPatrolIndex;
    private float lastAttackTime;

    private void Awake()
    {
        Animator = Visual.GetComponent<Animator>();
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
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
        SwitchState(EnemyState.Idle);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState();
        }
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
            Debug.LogError($"未找到状态：{newState}，请检查状态枚举！");
        }
    }

    public bool IsPlayerInDetectionRange()
    {
        if (playerTarget == null) return false;
        float distance = Vector2.Distance(transform.position, playerTarget.position);
        return distance <= DetectionRadius;
    }

    public bool IsPlayerInAttackRange()
    {
        if (playerTarget == null) return false;
        float distance = Vector2.Distance(transform.position, playerTarget.position);
        return distance <= AttackRange;
    }

    public Transform GetPlayerTarget()
    {
        return playerTarget;
    }

    public void MoveTowardsTarget(Vector2 targetPosition, float speed)
    {
        Vector2 direction = (targetPosition - Rigidbody.position).normalized;
        Rigidbody.MovePosition(Rigidbody.position + direction * speed * Time.fixedDeltaTime);
        
        if (direction.x > 0)
        {
            Visual.transform.localScale = new Vector3(Mathf.Abs(Visual.transform.localScale.x), Visual.transform.localScale.y, Visual.transform.localScale.z);
        }
        else if (direction.x < 0)
        {
            Visual.transform.localScale = new Vector3(-Mathf.Abs(Visual.transform.localScale.x), Visual.transform.localScale.y, Visual.transform.localScale.z);
        }
    }

    public Vector2 GetNextPatrolPoint()
    {
        if (PatrolPoints == null || PatrolPoints.Length == 0)
        {
            return Rigidbody.position;
        }
        return PatrolPoints[currentPatrolIndex].position;
    }

    public void MoveToNextPatrolPoint()
    {
        if (PatrolPoints == null || PatrolPoints.Length == 0) return;
        
        Vector2 targetPos = PatrolPoints[currentPatrolIndex].position;
        MoveTowardsTarget(targetPos, MoveSpeed);

        if (Vector2.Distance(Rigidbody.position, targetPos) < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % PatrolPoints.Length;
        }
    }

    public bool CanAttack()
    {
        return Time.time - lastAttackTime >= AttackCooldown;
    }

    public void PerformAttack()
    {
        lastAttackTime = Time.time;
        if (playerTarget != null)
        {
            PlayerController player = playerTarget.GetComponent<PlayerController>();
            if (player != null)
            {
                Debug.Log($"敌人对玩家造成 {Damage} 点伤害！");
            }
        }
    }

    public void StopMovement()
    {
        Rigidbody.velocity = Vector2.zero;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, DetectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
