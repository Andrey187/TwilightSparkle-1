public class TakingDamageState : EnemyState
{
    public override void Enter(IEnemy enemy)
    {
        enemy.MeshAnimator.Play("GetHit");
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
