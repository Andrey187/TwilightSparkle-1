public class AttackingState : EnemyState
{
    public override void Enter(BaseEnemy enemy)
    {
        enemy.meshAnimator.Play("Attack01");
    }

    public override void Exit(BaseEnemy enemy)
    {
    }

    public override void Update(BaseEnemy enemy)
    {
        // Attack the player
    }
}
