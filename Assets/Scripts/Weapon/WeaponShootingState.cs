using UnityEngine;

public class WeaponShootingState : WeaponBaseState
{
    public WeaponShootingState(WeaponController controller) : base(controller, WeaponState.Shooting)
    {
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (Input.GetMouseButton(0))
        {
            weaponController.Fire();
        }

        if (weaponController.RuntimeData.currentClipAmmo <= 0 || weaponController.RuntimeData.isReloading)
        {
            weaponController.SwitchState(WeaponState.NoShooting);
        }
    }

    public override void Exit()
    {
    }
}