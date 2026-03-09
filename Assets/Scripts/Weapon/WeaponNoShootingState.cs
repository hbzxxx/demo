// WeaponNoShootingState.cs£¨ÖØ¹¹ºó£©
using UnityEngine;

public class WeaponNoShootingState : WeaponBaseState
{
    public WeaponNoShootingState(WeaponController controller) : base(controller, WeaponState.NoShooting)
    {
    }

    public override void Enter()
    {
    }

    public override void Update()
    {
        if (weaponController.RuntimeData.currentClipAmmo > 0&& !weaponController.RuntimeData.isReloading)
        {
            weaponController.SwitchState(WeaponState.Shooting);
        }
    }

    public override void Exit()
    {
    }
}