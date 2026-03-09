using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyController enemy) : base(enemy, EnemyState.Chase) { }

    public override void EnterState()
    {
        if (enemy.Animator != null)
        {
            enemy.Animator.SetBool("IsChase", true);
            enemy.Animator.SetBool("IsAttack", false);
        }
    }

    public override void UpdateState()
    {
        if (enemy.GetPlayerTarget() == null)
        {
            enemy.SwitchState(EnemyState.Idle);
            return;
        }

        if (enemy.IsPlayerInAttackRange())
        {
            enemy.SwitchState(EnemyState.Attack);
            return;
        }

        if (!enemy.IsPlayerInDetectionRange())
        {
            enemy.SwitchState(EnemyState.Idle);
            return;
        }

        enemy.MoveTowardsTarget(enemy.GetPlayerTarget().position, enemy.ChaseSpeed);
    }

    public override void ExitState()
    {
    }
}
