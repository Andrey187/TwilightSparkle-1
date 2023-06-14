public class AttackingState : EnemyState
{
    private const string AnimationTrigger = "Attack";

    public override void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetTrigger(AnimationTrigger);
    }

    public override void Exit(BaseEnemy enemy)
    {
    }

    public override void Update(BaseEnemy enemy)
    {
        // Attack the player
    }
}
