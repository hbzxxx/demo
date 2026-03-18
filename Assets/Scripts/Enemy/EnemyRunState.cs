using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class EnemyRunState : EnemyBaseState
{
    public EnemyRunState(EnemyController controller) : base(controller, EnemyState.Run)
    {
    }

    public override void EnterState()
    {
        Debug.Log("进行追逐");
        MoveToPlayer();
    }
    public override void UpdateState()
    {
        if (controller.isDie) return;
        float dist = Vector2.Distance(controller.transform.position, controller.player.position);//与玩家的距离
        if (dist > enemyData.detectRange)//在追踪范围外
        {
            controller.SwitchState(EnemyState.Idle);
        }
        else
        {
            MoveToPlayer();
        }
    }

    public void MoveToPlayer()
    {
        Vector2 dir = (controller.player.position - controller.transform.position).normalized;
        controller.rigidbody2D.velocity = dir * enemyData.moveSpeed;
    }
    public override void ExitState()
    {
        throw new System.NotImplementedException();
    }
}
