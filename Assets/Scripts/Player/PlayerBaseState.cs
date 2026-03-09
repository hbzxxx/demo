public abstract class PlayerBaseState
{
    protected PlayerController player;
    protected PlayerState stateName;

    public PlayerBaseState(PlayerController player, PlayerState stateName)
    {
        this.player = player;
        this.stateName = stateName;
    }

    // 进入状态时执行（只执行一次）
    public abstract void EnterState();
    // 状态持续时每帧执行
    public abstract void UpdateState();
    // 退出状态时执行（只执行一次）
    public abstract void ExitState();
}