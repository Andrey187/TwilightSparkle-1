public class AttackingState : EnemyState
{
    public override void Enter(IEnemy enemy)
    {
        enemy.MeshAnimator.Play("Attack01");
    }

    public override void Exit(IEnemy enemy)
    {
    }

    public override void Update(IEnemy enemy)
    {
        // Attack the player
    }
}
