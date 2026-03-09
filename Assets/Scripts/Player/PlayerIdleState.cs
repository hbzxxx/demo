using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerController player) : base(player, PlayerState.Idle) { }

    public override void EnterState()
    {
        player.Rigidbody.velocity = new Vector3(0, player.Rigidbody.velocity.y, 0);
        player.Animator.SetBool("IsRun", false);
    }

    public override void UpdateState()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            player.SwitchState(PlayerState.Run);
        }
    }

    public override void ExitState()
    {
    }
}