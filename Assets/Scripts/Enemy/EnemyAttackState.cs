using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyController controller) : base(controller, EnemyState.Attack)
    {
    }

    public override void EnterState()
    {
        controller.rigidbody2D.velocity = Vector3.zero;
    }
    public override void UpdateState()
    {
        Collider2D hit = Physics2D.OverlapCircle(controller.transform.position, enemyData.attackRange, controller.playerlayerMask);
        if (hit != null)
        {
            if ( (Time.time - enemyData.attackTime) < enemyData.attackCD )
            {
                Debug.Log("ssssssssssss");
                enemyData.attackTime= Time.time;//记录最后一次攻击时间
                controller.animator.SetTrigger("Attack"); //攻击的动画
                //玩家受到攻击的函数
                PlayerController.Instance.Hit();
            }
        }
        else
        {
            controller.SwitchState(EnemyState.Run);//不在攻击范围
        }
    }
    public override void ExitState()
    {
    }

}
