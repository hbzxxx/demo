using UnityEngine;

public class EnemyPatrolState : EnemyBaseState
{
    public EnemyPatrolState(EnemyController enemy) : base(enemy, EnemyState.Patrol) { }

    public override void EnterState()
    {
        if (enemy.Animator != null)
        {
            enemy.Animator.SetBool("IsChase", false);
            enemy.Animator.SetBool("IsAttack", false);
        }
    }

    public override void UpdateState()
    {
        if (enemy.IsPlayerInDetectionRange())
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        enemy.MoveToNextPatrolPoint();
    }

    public override void ExitState()
    {
    }
}
