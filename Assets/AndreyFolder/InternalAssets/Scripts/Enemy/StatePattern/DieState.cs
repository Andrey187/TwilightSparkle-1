public class DieState : EnemyState
{
    public override void Enter(IEnemy enemy)
    {
        enemy.MeshAnimator.Play("Die");
    }

    public override void Exit(IEnemy enemy)
    {
        // Return to previous state or transition to a different state
    }

    public override void Update(IEnemy enemy)
    {
        // Handle any logic related to taking damage
    }
}
