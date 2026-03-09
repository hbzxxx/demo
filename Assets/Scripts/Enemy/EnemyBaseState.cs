public abstract class EnemyBaseState
{
    protected EnemyController enemy;
    protected EnemyState stateName;

    public EnemyBaseState(EnemyController enemy, EnemyState stateName)
    {
        this.enemy = enemy;
        this.stateName = stateName;
    }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
}
