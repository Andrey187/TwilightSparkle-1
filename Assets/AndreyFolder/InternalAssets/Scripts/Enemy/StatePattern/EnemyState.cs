public abstract class EnemyState
{
    public abstract void Enter(IEnemy enemy);
    public abstract void Exit(IEnemy enemy);
    public abstract void Update(IEnemy enemy);
}
