public class WalkingState : EnemyState
{
    private const string AnimationTrigger = "WalkFWD";

    public override void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetTrigger(AnimationTrigger);
    }

    public override void Exit(BaseEnemy enemy)
    {
    }

    public override void Update(BaseEnemy enemy)
    {
        // Move towards the target position
    }
}
