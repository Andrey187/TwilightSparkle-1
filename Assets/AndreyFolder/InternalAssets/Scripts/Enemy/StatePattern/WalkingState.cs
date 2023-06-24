public class WalkingState : EnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.meshAnimator.Play("WalkFWD");
    }

    public override void Exit(BaseEnemy enemy)
    {
    }

    public override void Update(BaseEnemy enemy)
    {
        // Move towards the target position
    }
}
