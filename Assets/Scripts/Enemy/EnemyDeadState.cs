using UnityEngine;

public class EnemyDeadState : EnemyBaseState
{

    public EnemyDeadState(EnemyController controller) : base(controller, EnemyState.Attack) { }

    public override void EnterState()
    {
        enemyData.isDead = true;
        controller.rigidbody2D.velocity = Vector3.zero;
        controller.animator.SetTrigger("Dead");
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
    }
}
