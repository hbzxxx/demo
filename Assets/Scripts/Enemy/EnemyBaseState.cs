public abstract class EnemyBaseState
{
    protected EnemyController controller;  
    protected EnemyData enemyData;             // 敌人的数据
    public readonly EnemyState stateType;      // 标记当前状态类型

    public EnemyBaseState(EnemyController controller, EnemyState stateType)
    {
        this.controller = controller;
        this.enemyData = controller.inGameEnemyData;
        this.stateType = stateType;
    }

    // 进入状态时执行（只执行一次）
    public abstract void EnterState();
    // 状态持续时每帧执行
    public abstract void UpdateState();
    // 退出状态时执行（只执行一次）
    public abstract void ExitState();
}
