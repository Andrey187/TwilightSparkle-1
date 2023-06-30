public class DieState : EnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.meshAnimator.Play("Die");
    }

    public override void Exit(BaseEnemy enemy)
    {
        // Return to previous state or transition to a different state
    }

    public override void Update(BaseEnemy enemy)
    {
        // Handle any logic related to taking damage
    }
}
