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
    }
    public override void UpdateState()
    {
        if (enemyData.isDead) return;
        float dist = Vector2.Distance(controller.transform.position, controller.player.position);//与玩家的距离
        if (dist > enemyData.detectRange)//在追踪范围外
        {
            controller.SwitchState(EnemyState.Idle);
        }
        else
        {
            MoveToPlayer(dist);
        }
    }

    public void MoveToPlayer(float dist)
    {
        if (dist <= enemyData.attackRange)
        {
            controller.SwitchState(EnemyState.Attack);
        }
        else
        {
            Vector2 dir = (controller.player.position - controller.transform.position).normalized;
            controller.rigidbody2D.velocity = dir * enemyData.moveSpeed;
        }
    }
    public override void ExitState()
    {
    }
}
