using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float waitTimer;

    public EnemyIdleState(EnemyController enemy) : base(enemy, EnemyState.Idle) { }

    public override void EnterState()
    {
        enemy.StopMovement();
        if (enemy.Animator != null)
        {
            enemy.Animator.SetBool("IsChase", false);
            enemy.Animator.SetBool("IsAttack", false);
        }
        waitTimer = 0f;
    }

    public override void UpdateState()
    {
        waitTimer += Time.deltaTime;

        if (enemy.IsPlayerInDetectionRange())
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        if (enemy.PatrolPoints != null && enemy.PatrolPoints.Length > 0)
        {
            if (waitTimer >= enemy.PatrolWaitTime)
            {
                enemy.SwitchState(EnemyState.Patrol);
            }
        }
    }

    public override void ExitState()
    {
    }
}
