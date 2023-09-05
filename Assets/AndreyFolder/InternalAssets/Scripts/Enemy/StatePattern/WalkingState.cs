public class WalkingState : EnemyState
{
    public override void Enter(IEnemy enemy)
    {
        enemy.MeshAnimator.Play("WalkFWD");
    }

    public override void Exit(IEnemy enemy)
    {
    }

    public override void Update(IEnemy enemy)
    {
        // Move towards the target position
    }
}
