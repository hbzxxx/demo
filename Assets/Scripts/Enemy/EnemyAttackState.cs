using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    private float attackTimer;

    public EnemyAttackState(EnemyController enemy) : base(enemy, EnemyState.Attack) { }

    public override void EnterState()
    {
        enemy.StopMovement();
        if (enemy.Animator != null)
        {
            enemy.Animator.SetBool("IsChase", false);
            enemy.Animator.SetBool("IsAttack", true);
        }
        attackTimer = 0f;
    }

    public override void UpdateState()
    {
        if (enemy.GetPlayerTarget() == null)
        {
            enemy.SwitchState(EnemyState.Idle);
            return;
        }

        if (!enemy.IsPlayerInAttackRange())
        {
            enemy.SwitchState(EnemyState.Chase);
            return;
        }

        attackTimer += Time.deltaTime;

        if (enemy.CanAttack() && attackTimer >= 0.5f)
        {
            enemy.PerformAttack();
            attackTimer = 0f;
        }
    }

    public override void ExitState()
    {
    }
}
