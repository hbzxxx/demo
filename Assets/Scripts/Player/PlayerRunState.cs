using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerBaseState
{
    public Vector2 _move;//移动角度

    public PlayerRunState(PlayerController player) : base(player, PlayerState.Run) { }

    public override void EnterState()
    {
        player.Rigidbody.velocity = new Vector3(0, player.Rigidbody.velocity.y, 0);
        player.Animator.SetBool("IsRun", true);
    }

    public override void UpdateState()
    {
        MovePosition();
        if (_move.x == 0 && _move.y == 0)
        {
            player.SwitchState(PlayerState.Idle);
        }
        else
        {
            player.MovePlayer(_move);
        }
    }

    private void MovePosition()
    {
        _move.x = Input.GetAxisRaw("Horizontal");
        _move.y = Input.GetAxisRaw("Vertical");
    }

    public override void ExitState()
    {
        Debug.Log("退出移动状态");
    }
}