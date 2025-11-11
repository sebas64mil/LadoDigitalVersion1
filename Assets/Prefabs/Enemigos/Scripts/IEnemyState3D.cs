public interface IEnemyState
{
    void Enter(EnemyController enemy);
    void Update(EnemyController enemy);
    void Exit(EnemyController enemy);
}

public abstract class EnemyState : IEnemyState
{
    public virtual void Enter(EnemyController enemy) { }
    public virtual void Exit(EnemyController enemy) { }
    public abstract void Update(EnemyController enemy);
}