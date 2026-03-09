public abstract class WeaponBaseState
{
    protected WeaponController weaponController; // 持有武器控制器引用
    protected WeaponDataSO weaponData;          // 直接持有武器配置数据
    public readonly WeaponState stateType;      // 标记当前状态类型

    public WeaponBaseState(WeaponController controller, WeaponState stateType)
    {
        weaponController = controller;
        weaponData = controller.WeaponDataSO;   // 从控制器获取配置，避免重复传参
        this.stateType = stateType;
    }

    // 进入状态（仅执行一次）
    public abstract void Enter();
    // 帧更新（每帧执行）
    public abstract void Update();
    // 退出状态（仅执行一次）
    public abstract void Exit();
}