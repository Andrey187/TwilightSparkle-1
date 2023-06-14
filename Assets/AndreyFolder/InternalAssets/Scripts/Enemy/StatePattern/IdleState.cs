public class IdleState : EnemyState
{
    private const string AnimationTrigger = "IdleNormal";

    public override void Enter(BaseEnemy enemy)
    {
        enemy.Animator.SetTrigger(AnimationTrigger);
    }

    public override void Exit(BaseEnemy enemy)
    {
    }

    public override void Update(BaseEnemy enemy)
    {
    }
}
