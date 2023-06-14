public abstract class EnemyState
{
    public abstract void Enter(BaseEnemy enemy);
    public abstract void Exit(BaseEnemy enemy);
    public abstract void Update(BaseEnemy enemy);
}
