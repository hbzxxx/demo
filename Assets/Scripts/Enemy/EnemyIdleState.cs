using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyController controller) : base(controller, EnemyState.Idle)
    {
    }

    public override void EnterState()
    {
        Debug.Log("玩家不在附近");
        controller.rigidbody2D.velocity = Vector3.zero;
    }
    public override void UpdateState()
    {
        if (enemyData.isDead) return;
        float dist = Vector2.Distance(controller.transform.position, controller.player.position);//与玩家的距离
        if (dist <= enemyData.detectRange)//在追踪范围内
        {
            controller.SwitchState(EnemyState.Run);
        }
    }
    public override void ExitState()
    {
    }
}
